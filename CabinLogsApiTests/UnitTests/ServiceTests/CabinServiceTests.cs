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

        // Assert
        var cabins = await _sut.GetCabins();
        cabins.Should().NotBeNull();
        cabins.Should().HaveCount(1);
    }

    [Fact]
    public async Task AddCabin_WithValidCabin_ReturnsTrue()
    {
        // Arrange
        var ctx = _ctxBuilder.WithCabins().Build();
        _sut = new CabinService(ctx);
        var cabin = new Cabin
        {
            id = 2,
            created_at = DateTime.UtcNow,
            name = "001",
            maxCapacity = 4,
            regularPrice = 400,
            discount = 0,
            description = "This is a test",
            image = null
        };

        // Act
        var result = await _sut.AddCabin(cabin);

        // Assert
        var getCabins = await _sut.GetCabins();
        getCabins.Should().NotBeNull();
        getCabins.Should().HaveCount(2);

    }

    [Fact]
    public async Task AddCabin_WithDuplicateId_ReturnsFalse()
    {
        // Arrange
        var ctx = _ctxBuilder.WithCabins().Build();
        _sut = new CabinService(ctx);
        var cabin = new Cabin
        {
            id = 1,
            created_at = DateTime.UtcNow,
            name = "001",
            maxCapacity = 4,
            regularPrice = 400,
            discount = 0,
            description = "This is a test",
            image = null
        };

        // Act
        var result = await _sut.AddCabin(cabin);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateCabin_WithValidData_ReturnsTrue()
    {
        // Arrange
        var ctx = _ctxBuilder.WithCabins().Build();
        _sut = new CabinService(ctx);
        var initialCabin = new Cabin
        {
            id = 2,
            created_at = DateTime.UtcNow,
            name = "Test cabin",
            maxCapacity = 4,
            regularPrice = 400,
            discount = 0,
            description = "None",
            image = null
        };
        await _sut.AddCabin(initialCabin);
        var updatedCabin = new Cabin
        {
            name = "Updated Cabin",
            maxCapacity = 6,
            regularPrice = 450,
            discount = 10,
            description = "Hello world!",
            image = "new_cabin.png"
        };

        // Act
        var result = await _sut.UpdateCabin(initialCabin.id, updatedCabin);

        // Assert
        result.Should().BeTrue();

        var getCabin = await ctx.Cabins.FindAsync(initialCabin.id);
        getCabin.Should().NotBeNull();
        getCabin?.name.Should().Be(updatedCabin.name);
        getCabin?.maxCapacity.Should().Be(updatedCabin.maxCapacity);
        getCabin?.regularPrice.Should().Be(updatedCabin.regularPrice);
        getCabin?.discount.Should().Be(updatedCabin.discount);
        getCabin?.description.Should().Be(updatedCabin.description);
        getCabin?.image.Should().Be(updatedCabin.image);
    }

    [Fact]
    public async Task UpdateCabin_WithInvalidId_ReturnsFalse()
    {
        // Arrange
        var ctx = _ctxBuilder.WithCabins().Build();
        _sut = new CabinService(ctx);
        var initialCabin = new Cabin
        {
            id = 2,
            created_at = DateTime.UtcNow,
            name = "Test cabin",
            maxCapacity = 4,
            regularPrice = 400,
            discount = 0,
            description = "None",
            image = null
        };
        await _sut.AddCabin(initialCabin);
        var updatedCabin = new Cabin
        {
            name = "Updated Cabin",
            maxCapacity = 6,
            regularPrice = 450,
            discount = 10,
            description = "Hello world!",
            image = "new_cabin.png"
        };

        // Act
        var result = await _sut.UpdateCabin(3, updatedCabin);

        // Assert
        result.Should().BeFalse();
    }
}

