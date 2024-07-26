using CabinLogsApi.Models;
using Microsoft.EntityFrameworkCore;

public class SettingService : ISettingService
{
	private readonly ApplicationDbContext _context;
	public SettingService(ApplicationDbContext context)
	{
		_context = context;
	}
	public async Task<List<Setting>> GetSettings()
	{
		try
		{
			var settings = await _context.Settings.ToListAsync();
			if (settings.Count == 0)
			{
				return new List<Setting>();
			}
			return settings;
		}
		catch (Exception ex)
		{
			throw new Exception($"Error occurred when accessing Database: {ex.Message}");
		}
	}
}