using CabinLogsApi.Controllers;
using CabinLogsApi.DTO.Cabins;
using CabinLogsApi.Models;
using CabinLogsApiTests.Fakes;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;


namespace CabinLogsApiTests.UnitTests.ControllerTests;

public class CabinsControllerTests : IDisposable
{
    private readonly ApplicationDbContextFakeBuilder _ctxBuild = new ();
    public void Dispose()
    {
        _ctxBuild.Dispose();
    }

    [Fact]
    public async Task GetAllCabins_WithCabins_ReturnsOkWithList()
    {
        // Arrange
        var context = _ctxBuild.WithCabins().WithGuests().WithSettings().Build();
        var cabinService = new CabinService(context);
        var _sut = new CabinsController(cabinService);

        // Act
        var result = await _sut.GetAllCabins();

        // Assert
        var getResult = result as OkObjectResult;
        getResult.Should().NotBeNull();
        getResult?.StatusCode.Should().Be(200);
        var returnedCabins = getResult?.Value as List<CabinDTO>;
        returnedCabins.Should().NotBeNull();

        returnedCabins.Should().BeEquivalentTo(new List<CabinDTO>
        {
            new CabinDTO
            {
                Id = 1,
                created_at = DateTime.UtcNow,
                Name = "001",
                MaxCapacity = 2,
                RegularPrice = 250,
                Discount = 50,
                Description = "Small luxury cab in the woods",
                Image = null,
            },
        }, options => options.Excluding(c => c.created_at));
    }

    [Fact]
    public async Task GetAllCabins_WithoutCabins_ReturnsOkWithEmptyList()
    {
        // Arrange
        var context = _ctxBuild.WithGuests().WithSettings().Build();
        var cabinService = new CabinService(context);
        var _sut = new CabinsController(cabinService);

        // Act
        var result = await _sut.GetAllCabins();

        // Assert
        var getResult = result as OkObjectResult;
        getResult.Should().NotBeNull();
        getResult?.StatusCode.Should().Be(200);
        var returnedCabins = getResult?.Value as List<CabinDTO>;
        returnedCabins.Should().NotBeNull();
        returnedCabins.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCabin_WithValidId_ReturnsCorrectCabin()
    {
        // Arrange
        var context = _ctxBuild.WithCabins().WithGuests().WithSettings().Build();
        var cabinService = new CabinService(context);
        var _sut = new CabinsController(cabinService);

        // Act
        var result = await _sut.GetCabin(1);

        // Assert
        var getResult = result as OkObjectResult;
        getResult.Should().NotBeNull();
        getResult?.StatusCode.Should().Be(200);
        var returnedCabin = getResult?.Value as Cabin;
        returnedCabin.Should().NotBeNull();
        returnedCabin.Should().BeEquivalentTo(new Cabin
        {
            id = 1,
            created_at = DateTime.UtcNow,
            name = "001",
            maxCapacity = 2,
            regularPrice = 250,
            discount = 50,
            description = "Small luxury cab in the woods",
            image = null,
        }, options => options.Excluding(c => c.created_at));
    }
}

