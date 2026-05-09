using Microsoft.AspNetCore.Mvc;
using rest_api_apbd.Data;
using rest_api_apbd.Models;

namespace rest_api_apbd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetReservations([FromQuery] DateTime? date, [FromQuery] string? status, [FromQuery] int? roomId)
    {
        var query = DataStore.Reservations.AsQueryable();

        if (date.HasValue) query = query.Where(r => r.Date.Date == date.Value.Date);
        if (!string.IsNullOrEmpty(status)) query = query.Where(r => r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        if (roomId.HasValue) query = query.Where(r => r.RoomId == roomId.Value);

        return Ok(query.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation == null) return NotFound($"Rezerwacja o id {id} nie istnieje.");
        
        return Ok(reservation);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Reservation newReservation)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == newReservation.RoomId);
        if (room == null) return NotFound("Wskazana sala nie istnieje.");
        if (!room.IsActive) return BadRequest("Wskazana sala jest obecnie nieaktywna.");

        if (IsTimeConflict(newReservation))
            return Conflict("Sala jest już zarezerwowana w podanym czasie.");

        newReservation.Id = DataStore.Reservations.Any() ? DataStore.Reservations.Max(r => r.Id) + 1 : 1;
        DataStore.Reservations.Add(newReservation);

        return CreatedAtAction(nameof(GetById), new { id = newReservation.Id }, newReservation);
    }

    [HttpPut("{id}")]
    public IActionResult Put([FromRoute] int id, [FromBody] Reservation updatedReservation)
    {
        var existingReservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (existingReservation == null) return NotFound($"Rezerwacja o id {id} nie istnieje.");

        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == updatedReservation.RoomId);
        if (room == null) return NotFound("Wskazana sala nie istnieje.");
        if (!room.IsActive) return BadRequest("Wskazana sala jest obecnie nieaktywna.");

        updatedReservation.Id = id; // Aby nie kolidowało samo z sobą w metodzie sprawdzającej
        if (IsTimeConflict(updatedReservation, id))
            return Conflict("Sala jest już zarezerwowana w podanym czasie.");

        existingReservation.RoomId = updatedReservation.RoomId;
        existingReservation.OrganizerName = updatedReservation.OrganizerName;
        existingReservation.Topic = updatedReservation.Topic;
        existingReservation.Date = updatedReservation.Date;
        existingReservation.StartTime = updatedReservation.StartTime;
        existingReservation.EndTime = updatedReservation.EndTime;
        existingReservation.Status = updatedReservation.Status;

        return Ok(existingReservation);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation == null) return NotFound($"Rezerwacja o id {id} nie istnieje.");

        DataStore.Reservations.Remove(reservation);
        return NoContent();
    }

    // Metoda pomocnicza sprawdzająca kolizje
    private bool IsTimeConflict(Reservation res, int? excludeId = null)
    {
        return DataStore.Reservations.Any(r =>
            r.RoomId == res.RoomId &&
            r.Date.Date == res.Date.Date &&
            r.Id != excludeId &&
            res.StartTime < r.EndTime && 
            res.EndTime > r.StartTime
        );
    }
}