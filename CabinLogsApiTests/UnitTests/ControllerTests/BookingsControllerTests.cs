using CabinLogsApi.Controllers;
using CabinLogsApi.DTO.Bookings;
using CabinLogsApi.Models;
using CabinLogsApiTests.Fakes;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace CabinLogsApiTests.UnitTests.ControllerTests;

public class BookingsControllerTests : IDisposable
{
    private readonly ApplicationDbContextFakeBuilder _ctxBuilder = new();
    public void Dispose()
    {
        _ctxBuilder.Dispose();
    }

    [Fact]
    public async Task GetBookings_WithBookings_ReturnsList()
    {
        // Arrange
        var context = _ctxBuilder.WithBookings().Build();
        var bookingService = new BookingService(context);
        var _sut = new BookingsController(bookingService);

        // Act
        var result = await _sut.GetBookings();

        // Assert
        var getResult = result as OkObjectResult;
        getResult.Should().NotBeNull();
        getResult?.StatusCode.Should().Be(200);
        var returnedBookings = getResult?.Value as List<BookingDTO>;
        returnedBookings.Should().NotBeNull();

        returnedBookings.Should().BeEquivalentTo(new List<BookingDTO>()
        {
            new BookingDTO
            {
                Id = 1,
                created_at = DateTime.UtcNow,
                StartDate = DateTime.Parse("2024-07-22 10:00:00"),
                EndDate = DateTime.Parse("2024-07-27 17:00:00"),
                NumberOfNights = 4,
                NumGuests = 2,
                CabinPrice = 300,
                ExtrasPrice = 120,
                TotalPrice = 420,
                Status = "unconfirmed",
                HasBreakfast = true,
                IsPaid = true,
                Observations = "I will arrive at 10 am",
                CabinId = 1,
                GuestId = 1,
            }
        }, options =>options.Excluding(b => b.created_at));
    }

    [Fact]
    public async Task GetBookings_WithoutBookings_ReturnsOkWithEmptyList()
    {
        // Arrange
        var context = _ctxBuilder.Build();
        var bookingService = new BookingService(context);
        var _sut = new BookingsController(bookingService);

        // Act
        var result = await _sut.GetBookings();

        // Assert
        var getResult = result as OkObjectResult;
        getResult.Should().NotBeNull();
        getResult?.StatusCode.Should().Be(200);
        var returnedBookings = getResult?.Value as List<BookingDTO>;
        returnedBookings.Should().NotBeNull();
        returnedBookings.Should().BeEmpty();
    }

    [Fact]
    public async Task GetBooking_WithCorrectId_ReturnsBooking()
    {
        // Arrange
        var context = _ctxBuilder.WithBookings().Build();
        var bookingService = new BookingService(context);
        var _sut = new BookingsController(bookingService);

        // Act
        var result = await _sut.GetBooking(1);

        // Assert
        var getResult = result as OkObjectResult;
        getResult.Should().NotBeNull();
        getResult?.StatusCode.Should().Be(200);
        var returnedBooking = getResult?.Value as Booking;
        returnedBooking.Should().NotBeNull();
        returnedBooking.Should().BeEquivalentTo(new Booking
        {
            id = 1,
            created_at = DateTime.UtcNow,
            startDate = DateTime.Parse("2024-07-22 10:00:00"),
            endDate = DateTime.Parse("2024-07-27 17:00:00"),
            numberOfNights = 4,
            numGuests = 2,
            cabinPrice = 300,
            extrasPrice = 120,
            totalPrice = 420,
            status = "unconfirmed",
            hasBreakfast = true,
            isPaid = true,
            observations = "I will arrive at 10 am",
            cabinId = 1,
            guestId = 1,
        }, options => options.Excluding(b => b.created_at));
    }

    [Fact]
    public async Task GetBooking_WithIncorrectId_ReturnsNotFound()
    {
        // Arrange
        var context = _ctxBuilder.WithBookings().Build();
        var bookingService = new BookingService(context);
        var _sut = new BookingsController(bookingService);

        // Act
        var result = await _sut.GetBooking(555);

        // Assert
        var getResult = result as NotFoundObjectResult;
        getResult?.Should().NotBeNull();
        getResult?.StatusCode.Should().Be(404);
    }
}
