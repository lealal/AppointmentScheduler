using AppointmentScheduler.Contracts;
using AppointmentScheduler.Exceptions;
using AppointmentScheduler.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v{version:apiVersion}/appointment")]
    [ApiVersion("1.0")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentManager appointmentManager;

        public AppointmentController(AppointmentManager manager)
        {
            appointmentManager = manager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointments()
        {
            var appointments = await appointmentManager.GetAppointmentsAsync();
            return Ok(appointments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var appointment = new Appointment
                {
                    Title = request.Title,
                    AppointmentReason = request.AppointmentReason,
                    StartDate = request.StartDate,
                    CustomerName = request.CustomerName,
                    CustomerEmail = request.CustomerEmail,
                    CustomerPhone = request.CustomerPhone
                };

                var appointmentId = await appointmentManager.CreateAppointAsync(appointment);

                return Ok(appointmentId);
            }
            catch (InvalidAppointmentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointment(Guid id)
        {
            var appointment = await appointmentManager.GetAppointmentAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(Guid id)
        {
            var result = await appointmentManager.DeleteAppointmentAsync(id);

            return Ok(result);
        }

        [HttpGet("find")]
        public async Task<IActionResult> FindAppointments([FromQuery] string contact)
        {
            if (string.IsNullOrEmpty(contact))
            {
                return BadRequest("Contact (phone or email) is required.");
            }

            var appointments = await appointmentManager.FindAppointmentsByContactAsync(contact);

            return Ok(appointments);
        }
    }
}