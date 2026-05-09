using System.ComponentModel.DataAnnotations;

namespace rest_api_apbd.Models;

public class Reservation : IValidatableObject
{
    public int Id { get; set; }
    
    [Required]
    public int RoomId { get; set; }

    [Required(ErrorMessage = "Imię i nazwisko organizatora jest wymagane.")]
    public string OrganizerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Temat rezerwacji jest wymagany.")]
    public string Topic { get; set; } = string.Empty;

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public TimeSpan StartTime { get; set; }

    [Required]
    public TimeSpan EndTime { get; set; }

    public string Status { get; set; } = "planned";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndTime <= StartTime)
        {
            yield return new ValidationResult(
                "Czas zakończenia musi być późniejszy niż czas rozpoczęcia.",
                new[] { nameof(EndTime) });
        }
    }
}