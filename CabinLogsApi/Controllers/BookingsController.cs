using CabinLogsApi.DTO.Bookings;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CabinLogsApi.Controllers;

[ApiController]
[Route("/bookings")]
// [EnableCors("AnyOrigin")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;
    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet(Name = "Get All Bookings")]
    public async Task<IActionResult> GetBookings()
    {
        try
        {
            var bookings = await _bookingService.GetBookings();
            var data = bookings.Select(b => new BookingDTO
            {
                Id = b.id,
                created_at = b.created_at,
                StartDate = b.startDate,
                EndDate = b.endDate,
                NumberOfNights = b.numberOfNights,
                NumGuests = b.numGuests,
                CabinPrice = b.cabinPrice,
                ExtrasPrice = b.extrasPrice,
                TotalPrice = b.totalPrice,
                Status = b.status,
                HasBreakfast = b.hasBreakfast,
                IsPaid = b.isPaid,
                Observations = b.observations,
                CabinId = b.cabinId,
                GuestId = b.guestId
            }).ToList();
            return Ok(data);
        }
        catch (Exception)
        {
            return StatusCode(500, "Failed to retrieve data from database.");
        }
    }

    [HttpGet("{id}", Name = "Get booking from id")]
    public async Task<IActionResult> GetBooking(int id)
    {
        try
        {
            var booking = await _bookingService.GetBooking(id);
            if (booking == null)
            {
                return StatusCode(404, "Booking not found.");
            }
            return new OkObjectResult(booking);
        }
        catch (Exception)
        {
            return StatusCode(500, "Failed to retrive booking from database.");
        }
    }
}
