using CabinLogsApi.Controllers;
using CabinLogsApi.DTO.Guests;
using CabinLogsApi.Models;
using CabinLogsApiTests.Fakes;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace CabinLogsApiTests.UnitTests.ControllerTests;

public class GuestsControllerTests : IDisposable
{
    private readonly ApplicationDbContextFakeBuilder _ctxBuild = new();
    public void Dispose()
    {
        _ctxBuild.Dispose();
    }

    [Fact]
    public async Task GetAllGuests_WithGuests_ReturnsOkWithList()
    {
        // Arrange
        var context = _ctxBuild.WithCabins().WithGuests().WithSettings().Build();
        var guestService = new GuestService(context);
        var _sut = new GuestsController(guestService);

        // Act
        var result = await _sut.GetAllGuests();

        // Assert
        var getResult = result as OkObjectResult;
        getResult.Should().NotBeNull();
        getResult?.StatusCode.Should().Be(200);
        var returnedGuests = getResult?.Value as List<GuestDTO>;

        returnedGuests.Should().BeEquivalentTo(new List<GuestDTO>
        {
            new GuestDTO
            {
                Id = 1,
                created_at = DateTime.UtcNow,
                FullName = "John Doe",
                NationalId = "dafdasfa",
                Nationality = "American",
                CountryFlag = null
            },
        }, options => options.Excluding(g => g.created_at));
    }

    [Fact]
    public async Task GetAllGuests_WithoutGuests_ReturnsOkWithEmptyList()
    {
        // Arrange
        var context = _ctxBuild.WithCabins().WithSettings().Build();
        var guestService = new GuestService(context);
        var _sut = new GuestsController(guestService);

        // Act
        var result = await _sut.GetAllGuests();

        // Assert
        var getResult = result as OkObjectResult;
        getResult.Should().NotBeNull();
        getResult?.StatusCode.Should().Be(200);
        var returnedGuests = getResult?.Value as List<GuestDTO>;
        returnedGuests.Should().NotBeNull();
        returnedGuests.Should().BeEmpty();
    }

    [Fact]
    public async Task GetGuest_WithValidId_ReturnsCorrectCabin()
    {
        // Arrange
        var context = _ctxBuild.WithGuests().WithCabins().WithSettings().Build();
        var guestService = new GuestService(context);
        var _sut = new GuestsController(guestService);

        // Act
        var result = await _sut.GetGuest(1);

        // Assert
        var getResult = result as OkObjectResult;
        getResult.Should().NotBeNull();
        getResult?.StatusCode.Should().Be(200);
        var returnedGuest = getResult?.Value as Guest;
        returnedGuest.Should().NotBeNull();
        returnedGuest.Should().BeEquivalentTo(new Guest
        {
            id = 1,
            created_at = DateTime.UtcNow,
            fullName = "John Doe",
            nationalId = "dafdasfa",
            nationality = "American",
            countryFlag = null,
        }, options => options.Excluding(g => g.created_at));
    }

    [Fact]
    public async Task GetGuest_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var context = _ctxBuild.WithGuests().WithCabins().WithSettings().Build();
        var guestService = new GuestService(context);
        var _sut = new GuestsController(guestService);

        // Act
        var result = await _sut.GetGuest(555);

        // Assert
        var getResult = result as NotFoundObjectResult;
        getResult.Should().NotBeNull();
        getResult?.StatusCode.Should().Be(404);
    }
}
