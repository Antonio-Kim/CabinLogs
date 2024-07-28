using CabinLogsApi.Models;
using Microsoft.EntityFrameworkCore;

public class CabinService : ICabinService
{
	private readonly ApplicationDbContext _context;

	public CabinService(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Cabin?> GetCabin(int id)
	{
		var cabin = await _context.Cabins.FirstOrDefaultAsync(c => c.id == id);
		return cabin;
	}

	public async Task<List<Cabin>> GetCabins()
	{
		try
		{
			var cabins = await _context.Cabins.ToListAsync();
			if (cabins.Count == 0)
			{
				return new List<Cabin>();
			}
			return cabins;
		}
		catch (Exception ex)
		{
			throw new Exception($"Error occurred when accessing Database: {ex.Message}");
		}
	}

	public async Task<bool> RemoveCabin(int id)
	{
		try
		{
			var cabin = await _context.Cabins.FindAsync(id);
			if (cabin == null)
			{
				return false;
			}
			_context.Cabins.Remove(cabin);
			await _context.SaveChangesAsync();
			return true;
		}
		catch (Exception)
		{
			throw new Exception("Error occurred when removing cabin.");
		}
	}
}