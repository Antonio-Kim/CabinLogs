using CabinLogsApi.Models;
using CabinLogsApiTests.Fakes;
using FluentAssertions;

namespace CabinLogsApiTests.UnitTests.ServiceTests;

public class SettingServiceTests : IDisposable
{
    private readonly ApplicationDbContextFakeBuilder _ctxBuilder = new();
    private SettingService? _sut;

    public void Dispose()
    {
        _ctxBuilder.Dispose();
    }

    [Fact]
    public async Task GetSettings_WithSettings_ReturnsList()
    {
        // Arrange 
        var ctx = _ctxBuilder.WithSettings().Build();
        _sut = new SettingService(ctx);

        // Act
        var settings = await _sut.GetSettings();

        // Assert
        settings.Should().NotBeEmpty();
        settings.Should().BeOfType<List<Setting>>();
        settings.Should().BeEquivalentTo(new List<Setting>
        {
            new Setting
            {
                id = 1,
                created_at = DateTime.UtcNow,
                minBookingLength = 3,
                maxBookingLength = 90,
                breakfastPrice = 15,
            }
        }, options => options.Excluding(g => g.created_at));
    }

    [Fact]
    public async Task GetSettings_NoSettings_ReturnsEmptyList()
    {
        // Arrange
        var ctx = _ctxBuilder.Build();
        _sut = new SettingService(ctx);

        // Act
        var settings = await _sut.GetSettings();

        // Assert
        settings.Should().NotBeNull();
        settings.Should().BeOfType<List<Setting>>();
        settings.Should().BeEmpty();
    }
}