using rest_api_apbd.Models;

namespace rest_api_apbd.Data;
public static class DataStore
{
    public static List<Room> Rooms { get; set; } = new()
    {
        new Room { Id = 1, Name = "Lab 204", BuildingCode = "B", Floor = 2, Capacity = 24, HasProjector = true, IsActive = true },
        new Room { Id = 2, Name = "Sala Wykładowa A", BuildingCode = "A", Floor = 0, Capacity = 100, HasProjector = true, IsActive = true },
        new Room { Id = 3, Name = "Pokój 101", BuildingCode = "C", Floor = 1, Capacity = 4, HasProjector = false, IsActive = true },
        new Room { Id = 4, Name = "Sala 305", BuildingCode = "B", Floor = 3, Capacity = 30, HasProjector = true, IsActive = false },
        new Room { Id = 5, Name = "Aula", BuildingCode = "A", Floor = 1, Capacity = 200, HasProjector = true, IsActive = true }
    };

    public static List<Reservation> Reservations { get; set; } = new()
    {
        new Reservation { Id = 1, RoomId = 1, OrganizerName = "Anna Kowalska", Topic = "Warsztaty .NET", Date = new DateTime(2026, 5, 10), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0), Status = "confirmed" },
        new Reservation { Id = 2, RoomId = 2, OrganizerName = "Jan Nowak", Topic = "Wykład: Architektura", Date = new DateTime(2026, 5, 10), StartTime = new TimeSpan(14, 0, 0), EndTime = new TimeSpan(16, 0, 0), Status = "planned" },
        new Reservation { Id = 3, RoomId = 1, OrganizerName = "Piotr Z", Topic = "Konsultacje", Date = new DateTime(2026, 5, 11), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(10, 0, 0), Status = "confirmed" },
        new Reservation { Id = 4, RoomId = 3, OrganizerName = "HR", Topic = "Rekrutacja", Date = new DateTime(2026, 5, 12), StartTime = new TimeSpan(11, 0, 0), EndTime = new TimeSpan(12, 30, 0), Status = "cancelled" }
    };
}