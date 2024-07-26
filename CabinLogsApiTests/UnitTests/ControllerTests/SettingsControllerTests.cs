using CabinLogsApi.Controllers;
using CabinLogsApi.DTO.Setting;
using CabinLogsApiTests.Fakes;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace CabinLogsApiTests.UnitTests.ControllerTests;

public class SettingsControllerTests : IDisposable
{
	private readonly ApplicationDbContextFakeBuilder _ctxBuilder = new();
	public void Dispose()
	{
		_ctxBuilder.Dispose();
	}

	[Fact]
	public async Task GetSettings_WithSettings_ReturnsOkWithList()
	{
		// Arrange
		var context = _ctxBuilder.WithSettings().Build();
		var settingService = new SettingService(context);
		var _sut = new SettingsController(settingService);

		// Act
		var result = await _sut.GetSettings();

		// Assert
		var getResult = result as OkObjectResult;
		getResult.Should().NotBeNull();
		getResult?.StatusCode.Should().Be(200);
		var returnedSettings = getResult?.Value as List<SettingDTO>;
		returnedSettings.Should().NotBeNull();

		returnedSettings.Should().BeEquivalentTo(new List<SettingDTO>
		{
			new SettingDTO
			{
                Id = 1,
                created_at = DateTime.UtcNow,
                MinBookingLength = 3,
                MaxBookingLength = 90,
                BreakfastPrice = 15,
            }
		}, options => options.Excluding(s => s.created_at));
	}

	[Fact]
	public async Task GetSettings_WithoutSettings_ReturnsEmptyList()
	{
        // Arrange
        var context = _ctxBuilder.Build();
        var settingService = new SettingService(context);
        var _sut = new SettingsController(settingService);

        // Act
        var result = await _sut.GetSettings();

        // Assert
        var getResult = result as OkObjectResult;
        getResult.Should().NotBeNull();
        getResult?.StatusCode.Should().Be(200);
        var returnedSettings = getResult?.Value as List<SettingDTO>;
        returnedSettings.Should().NotBeNull();

        returnedSettings.Should().BeEmpty();
	}
}