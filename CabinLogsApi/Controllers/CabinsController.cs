using CabinLogsApi.DTO.Cabins;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CabinLogsApi.Controllers;

[ApiController]
[Route("/cabins")]
[EnableCors("AnyOrigins")]
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

}