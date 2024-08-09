using CabinLogsApi.Models;
using CabinLogsApiTests.Fakes;
using FluentAssertions;

namespace CabinLogsApiTests.UnitTests.ServiceTests;

public class BookingServiceTests : IDisposable
{
    private readonly ApplicationDbContextFakeBuilder _ctxBuilder = new();
    private BookingService? _sut;

    public void Dispose()
    {
        _ctxBuilder.Dispose();
    }

    [Fact]
    public async Task GetBookings_WithBookings_ReturnsList()
    {
        // Arrange
        var ctx = _ctxBuilder.WithBookings().Build();
        _sut = new BookingService(ctx);

        // Act
        var cabins = await _sut.GetBookings();

        // Assert
        cabins.Should().NotBeEmpty();
        cabins.Should().BeOfType<List<Booking>>();
        cabins.Should().BeEquivalentTo(new List<Booking>
        {
            new Booking
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
            },
        }, options => options.Excluding(c => c.created_at));
    }

    [Fact]
    public async Task GetBookings_NoBookings_ReturnsEmptyList()
    {
        // Arrange
        var ctx = _ctxBuilder.Build();
        _sut = new BookingService(ctx);

        // Act
        var bookings = await _sut.GetBookings();

        // Assert
        bookings.Should().NotBeNull();
        bookings.Should().BeOfType<List<Booking>>();
        bookings.Should().BeEmpty();
    }

    [Fact]
    public async Task GetBooking_WithCorrectId_ReturnsBooking()
    {
        // Arrange
        var ctx = _ctxBuilder.WithBookings().Build();
        _sut = new BookingService(ctx);

        // Act
        var booking = await _sut.GetBooking(1);

        // Assert
        booking.Should().NotBeNull();
        booking.Should().BeOfType<Booking>();
        booking.Should().BeEquivalentTo(new Booking
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
    public async Task GetBooking_WithIncorrectId_ReturnsNull()
    {
        // Arrange
        var ctx = _ctxBuilder.WithBookings().Build();
        _sut = new BookingService(ctx);

        // Act
        var booking = await _sut.GetBooking(555);

        //
        booking.Should().BeNull();
    }
}