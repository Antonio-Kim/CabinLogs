using CabinLogsApi.Models;
using CabinLogsApiTests.Fakes;
using FluentAssertions;

namespace CabinLogsApiTests.UnitTests.ServiceTests;

public class GuestServiceTests : IDisposable
{
    private readonly ApplicationDbContextFakeBuilder _ctxBuilder = new();
    private GuestService? _sut;

    public void Dispose()
    {
        _ctxBuilder.Dispose();
    }

    [Fact]
    public async Task GetGuests_WithGuests_ReturnsList()
    {
        // Arrange
        var ctx = _ctxBuilder.WithGuests().Build();
        _sut = new GuestService(ctx);

        // Act
        var guests = await _sut.GetGuests();

        // Assert
        guests.Should().NotBeNullOrEmpty();
        guests.Should().BeOfType<List<Guest>>();
        guests.Should().BeEquivalentTo(new List<Guest>
        {
            new Guest
            {
                id = 1,
                created_at = DateTime.UtcNow,
                fullName = "John Doe",
                nationalId = "dafdasfa",
                nationality = "American",
                countryFlag = null,
            }
        }, options => options.Excluding(g => g.created_at));
    }

    [Fact]
    public async Task GetGuests_WithoutGuests_ReturnsEmptyList()
    {
        // Arrange
        var ctx = _ctxBuilder.Build();
        _sut = new GuestService(ctx);

        // Act
        var guests = await _sut.GetGuests();

        // Assert
        guests.Should().NotBeNull();
        guests.Should().BeEmpty();
    }

    [Fact]
    public async Task GetGuest_WithCorrectId_ReturnsCorrectGuest()
    {
        // Arrange
        var ctx = _ctxBuilder.WithGuests().Build();
        _sut = new GuestService(ctx);
        var expected = new Guest
        {
            id = 1,
            created_at = DateTime.UtcNow,
            fullName = "John Doe",
            nationalId = "dafdasfa",
            nationality = "American",
            countryFlag = null
        };

        // Act
        var guest = await _sut.GetGuest(1);

        // Assert
        guest.Should().NotBeNull();
        guest.Should().BeOfType(typeof(Guest));
        guest.Should().BeEquivalentTo(expected, options => options.Excluding(g => g.created_at));
    }

    [Fact]
    public async Task GetGuest_WithCorrectId_ReturnsGuest()
    {
        // Arrange
        var ctx = _ctxBuilder.WithGuests().Build();
        _sut = new GuestService(ctx);
        var expected = new Guest
        {
            id = 1,
            created_at = DateTime.UtcNow,
            fullName = "John Doe",
            nationalId = "dafdasfa",
            nationality = "American",
            countryFlag = null,
        };

        // Act
        var guest = await _sut.GetGuest(1);

        // Assert
        guest.Should().NotBeNull();
        guest.Should().BeOfType<Guest>();
    }
}
