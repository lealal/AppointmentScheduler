using AppointmentScheduler.Services;

namespace AppointmentScheduler.Managers
{
    public class AppointmentManager
    {
        private readonly AppointmentService appointmentService;

        public AppointmentManager(AppointmentService service)
        {
            appointmentService = service;
        }

        public async Task<List<Appointment>> GetAppointmentsAsync()
        {
            return await appointmentService.GetAppointmentsAsync();
        }

        public async Task<Guid> CreateAppointAsync(Appointment appointment)
        {
            return await appointmentService.CreateAppointmentAsync(appointment);
        }

        public async Task<Appointment> GetAppointmentAsync(Guid id)
        {
            return await appointmentService.GetAppointmentAsync(id);
        }

        public async Task<Guid?> DeleteAppointmentAsync(Guid id)
        {
            return await appointmentService.DeleteAppointAsync(id);
        }

        public async Task<List<Appointment>> FindAppointmentsByContactAsync(string contact)
        {
            return await appointmentService.FindAppointmentByContactAsync(contact);
        }
    }
}