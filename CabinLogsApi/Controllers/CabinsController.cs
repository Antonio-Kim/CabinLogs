using CabinLogsApi.DTO.Cabins;
using CabinLogsApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CabinLogsApi.Controllers;

[ApiController]
[Route("/cabins")]
public class CabinsController : ControllerBase
{
    private readonly ICabinService _cabinService;
    public CabinsController(ICabinService cabinService)
    {
        _cabinService = cabinService;
    }

    [HttpGet(Name = "Get a list of cabins")]
    public async Task<IActionResult> GetAllCabins()
    {
        try
        {
            var cabins = await _cabinService.GetCabins();
            var data = cabins.Select(c => new CabinDTO
            {
                Id = c.id,
                created_at = c.created_at,
                Name = c.name,
                MaxCapacity = c.maxCapacity,
                RegularPrice = c.regularPrice,
                Discount = c.discount,
                Description = c.description,
                Image = c.image,
            }).ToList();

            return new OkObjectResult(data);
        }
        catch (Exception)
        {
            return StatusCode(500, "Failed to retrieve data from database.");
        }
    }

    [HttpGet("{id}", Name = "Get a cabin")]
    public async Task<IActionResult> GetCabin(int id)
    {
        try
        {
            var cabin = await _cabinService.GetCabin(id);
            if (cabin == null)
            {
                return StatusCode(404, "Cabin Id not found");
            }
            return new OkObjectResult(cabin);
        }
        catch (Exception)
        {
            return StatusCode(500, "Something went wrong");
        }

    }

    [HttpDelete("{id}", Name = "Remove a cabin")]
    public async Task<IActionResult> DeleteCabin(int id)
    {
        try
        {
            var cabin = await _cabinService.RemoveCabin(id);
            if (cabin == false)
            {
                return NotFound();
            }
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Something went wrong");
        }
    }

    [HttpPost(Name = "Add a cabin")]
    public async Task<IActionResult> AddCabin([FromBody] CabinDTO cabin)
    {
        if (cabin == null)
        {
            return BadRequest("Cabin is empty.");
        }

        var newCabin = new Cabin
        {
            id = cabin.Id,
            created_at = DateTime.UtcNow,
            name = cabin.Name,
            maxCapacity = cabin.MaxCapacity,
            regularPrice = cabin.RegularPrice,
            discount = cabin.Discount,
            description = cabin.Description,
            image = cabin.Image
        };

        var result = await _cabinService.AddCabin(newCabin);
        if (result)
        {
            return CreatedAtAction(nameof(GetCabin), new { id = newCabin.id }, newCabin);
        }

        return Conflict("A cabin with same ID already exists.");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCabin(int id, [FromBody] Cabin updatedCabin)
    {
        if (updatedCabin == null)
        {
            return BadRequest("Cabin is empty.");
        }
        var result = await _cabinService.UpdateCabin(id, updatedCabin);
        if (result)
        {
            return NoContent();
        }

        return NotFound("Cabin not found.");
    }
}