using System.ComponentModel.DataAnnotations;

public class Appointment
{
    public Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public required string AppointmentReason { get; set;}
    
    public DateTime StartDate { get; set; }
    
    public required string CustomerName { get; set; }
    
    [EmailAddress]
    public required string CustomerEmail { get; set; }
    
    [Phone]
    public required string CustomerPhone { get; set; }
}