using System.ComponentModel.DataAnnotations;

namespace rest_api_apbd.Models;

public class Room
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nazwa sali jest wymagana.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kod budynku jest wymagany.")]
    public string BuildingCode { get; set; } = string.Empty;

    public int Floor { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Pojemność musi być większa od 0.")]
    public int Capacity { get; set; }

    public bool HasProjector { get; set; }
    
    public bool IsActive { get; set; }
}