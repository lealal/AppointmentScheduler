using AppointmentScheduler.Data;
using AppointmentScheduler.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Services
{
    public class AppointmentService
    {
        private readonly AppDbContext dbContext;
        private readonly ILogger<AppointmentService> logger;

        private readonly TimeSpan START_OF_WORK_DAY = new(9, 0, 0);
        private readonly TimeSpan END_OF_WORK_DAY = new(17, 0, 0);

        public AppointmentService(AppDbContext context, ILogger<AppointmentService> logger)
        {
            dbContext = context;
            this.logger = logger;
        }

        public async Task<List<Appointment>> GetAppointmentsAsync()
        {
            return await dbContext.Appointments.ToListAsync();
        }

        public async Task<Guid> CreateAppointmentAsync(Appointment appointment)
        {
            try
            {
                logger.LogInformation("Attemption to create appointment for {CustomerName} at {StartDate}", appointment.CustomerName, appointment.StartDate);
                var today = DateTime.Today;
                var endOfValidRange = today.AddDays(14);

                if (appointment.StartDate < today || appointment.StartDate > endOfValidRange)
                {
                    throw new InvalidAppointmentException("Appointments must be scheduled within a 2 week period from today");
                }

                var appointmentStartTime = appointment.StartDate.TimeOfDay;
                var appointmentEndTime = appointment.StartDate.AddMinutes(45).TimeOfDay;

                if (appointmentStartTime < START_OF_WORK_DAY || appointmentEndTime > END_OF_WORK_DAY || 
                    appointment.StartDate.DayOfWeek == DayOfWeek.Saturday ||
                    appointment.StartDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    throw new InvalidAppointmentException("Appointments can only be scheduled Monday to Friday between 9:00 AM and 5:00 PM");
                }

                var expectedEndTime = appointment.StartDate.AddMinutes(45);

                var overlappingAppointments = await dbContext.Appointments
                    .Where(a =>
                        a.StartDate >= today &&
                        a.StartDate <= endOfValidRange &&
                        a.StartDate < expectedEndTime &&
                        appointment.StartDate < a.StartDate.AddMinutes(45))
                    .ToListAsync();

                if (overlappingAppointments.Any())
                {
                    throw new InvalidAppointmentException("The selected appointment time ovelaps with an existing appointment");
                }

                dbContext.Appointments.Add(appointment);
                await dbContext.SaveChangesAsync();

                logger.LogInformation("Successfully created appointment for {CustomerName} with record: {Guid}", appointment.CustomerName, appointment.Id);
                
                return appointment.Id;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating appoinment for {CustomerNmae}", appointment.CustomerName);
                throw;
            }
        }

        public async Task<Appointment> GetAppointmentAsync(Guid id)
        {
            return await dbContext.Appointments.FindAsync(id);
        }

        public async Task<Guid?> DeleteAppointAsync(Guid id)
        {
            var appointment = await dbContext.Appointments.FindAsync(id);

            if (appointment == null) {
                return null;
            }

            dbContext.Appointments.Remove(appointment);
            await dbContext.SaveChangesAsync();

            return appointment.Id;
        }

        public async Task<List<Appointment>> FindAppointmentByContactAsync(string contact)
        {
            var appointments = await dbContext.Appointments
                .Where(appointment => appointment.CustomerEmail == contact || appointment.CustomerPhone == contact)
                .ToListAsync();

            return appointments;
        }
    }
}