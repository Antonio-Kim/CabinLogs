using CabinLogsApi.Models;
using CabinLogsApiTests.Fakes;
using FluentAssertions;

namespace CabinLogsApiTests.UnitTests.ServiceTests;

public class CabinServiceTests : IDisposable
{
    private readonly ApplicationDbContextFakeBuilder _ctxBuilder = new();
    private CabinService? _sut;

    public void Dispose()
    {
        _ctxBuilder.Dispose();
    }

    [Fact]
    public async Task GetCabins_ReturnsList()
    {
        // Arrange
        var ctx = _ctxBuilder.WithCabins().Build();
        _sut = new CabinService(ctx);

        // Act
        var cabins = await _sut.GetCabins();

        // Assert
        cabins.Should().NotBeEmpty();
        cabins.Should().BeOfType<List<Cabin>>();
        cabins.Should().BeEquivalentTo(new List<Cabin>
        {
            new Cabin
            {
                id = 1,
                created_at = DateTime.UtcNow,
                name = "001",
                maxCapacity = 2,
                regularPrice = 250,
                discount = 50,
                description = "Small luxury cab in the woods",
                image = null,
            },
        }, options => options.Excluding(c => c.created_at));
    }

    [Fact]
    public async Task GetCabins_NoCabins_ReturnsEmptyList()
    {
        // Arrange
        var ctx = _ctxBuilder.Build();
        _sut = new CabinService(ctx);

        // Act
        var cabins = await _sut.GetCabins();

        // Assert
        cabins.Should().NotBeNull();
        cabins.Should().BeOfType<List<Cabin>>();
        cabins.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCabin_WithCorrectId_ReturnsCabin()
    {
        // Arrange
        var ctx = _ctxBuilder.WithCabins().Build();
        _sut = new CabinService(ctx);

        // Act
        var cabin = await _sut.GetCabin(1);

        // Assert
        cabin.Should().NotBeNull();
        cabin.Should().BeOfType(typeof(Cabin));
        cabin.Should().BeEquivalentTo(new Cabin
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

    [Fact]
    public async Task GetCabin_WithIncorrectId_ReturnsNull()
    {
        // Arrange
        var ctx = _ctxBuilder.WithCabins().Build();
        _sut = new CabinService(ctx);

        // Act
        var cabin = await _sut.GetCabin(555);

        // Assert
        cabin.Should().BeNull();
    }

    [Fact]
    public async Task DeleteCabin_WithOneCabin_ReturnsTrue()
    {
        // Arrange
        var ctx = _ctxBuilder.WithCabins().Build();
        _sut = new CabinService(ctx);

        // Act
        var result = await _sut.RemoveCabin(1);

        // Assert
        result.Should().BeTrue();
        var cabins = await _sut.GetCabins();
        cabins.Should().NotBeNull();
        cabins.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteCabin_WithEmptyList_ReturnsFalse()
    {
        // Arrange
        var ctx = _ctxBuilder.Build();
        _sut = new CabinService(ctx);

        // Act
        var result = await _sut.RemoveCabin(1);

        // Assert
        result.Should().BeFalse();
        var cabins = await _sut.GetCabins();
        cabins.Should().NotBeNull();
        cabins.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteCabin_WithInvalidId_ReturnsFalse()
    {
        // Arrange
        var ctx = _ctxBuilder.WithCabins().Build();
        _sut = new CabinService(ctx);

        // Act
        var result = await _sut.RemoveCabin(5);
        var cabins = await _sut.GetCabins();
        cabins.Should().NotBeNull();
        cabins.Should().HaveCount(1);
    }
}

