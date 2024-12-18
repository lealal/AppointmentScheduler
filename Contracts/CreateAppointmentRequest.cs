using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduler.Contracts
{
    public class CreateAppointmentRequest
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string AppointmentReason { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; }

        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; }

        [Required]
        [Phone]
        public string CustomerPhone { get; set; }
    }
}