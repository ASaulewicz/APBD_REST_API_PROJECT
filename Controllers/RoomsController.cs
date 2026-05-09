using Microsoft.AspNetCore.Mvc;
using rest_api_apbd.Data;
using rest_api_apbd.Models;

namespace rest_api_apbd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetRooms([FromQuery] int? minCapacity, [FromQuery] bool? hasProjector, [FromQuery] bool? activeOnly)
    {
        var query = DataStore.Rooms.AsQueryable();

        if (minCapacity.HasValue) query = query.Where(r => r.Capacity >= minCapacity.Value);
        if (hasProjector.HasValue) query = query.Where(r => r.HasProjector == hasProjector.Value);
        if (activeOnly.HasValue && activeOnly.Value) query = query.Where(r => r.IsActive);

        return Ok(query.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null) return NotFound($"Sala o id {id} nie istnieje.");
        
        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public IActionResult GetByBuilding([FromRoute] string buildingCode)
    {
        var rooms = DataStore.Rooms.Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase)).ToList();
        if (!rooms.Any()) return NotFound($"Nie znaleziono sal w budynku {buildingCode}.");

        return Ok(rooms);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Room newRoom)
    {
        newRoom.Id = DataStore.Rooms.Any() ? DataStore.Rooms.Max(r => r.Id) + 1 : 1;
        DataStore.Rooms.Add(newRoom);
        
        return CreatedAtAction(nameof(GetById), new { id = newRoom.Id }, newRoom);
    }

    [HttpPut("{id}")]
    public IActionResult Put([FromRoute] int id, [FromBody] Room updatedRoom)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null) return NotFound($"Sala o id {id} nie istnieje.");

        room.Name = updatedRoom.Name;
        room.BuildingCode = updatedRoom.BuildingCode;
        room.Floor = updatedRoom.Floor;
        room.Capacity = updatedRoom.Capacity;
        room.HasProjector = updatedRoom.HasProjector;
        room.IsActive = updatedRoom.IsActive;

        return Ok(room);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null) return NotFound($"Sala o id {id} nie istnieje.");

        // Sprawdzenie, czy są przypisane rezerwacje
        if (DataStore.Reservations.Any(res => res.RoomId == id))
        {
            return Conflict("Nie można usunąć sali, ponieważ istnieją powiązane z nią rezerwacje.");
        }

        DataStore.Rooms.Remove(room);
        return NoContent();
    }
}