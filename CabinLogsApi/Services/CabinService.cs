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
		var cabins = await _context.Cabins.ToListAsync();
		if (cabins.Count == 0)
		{
			return new List<Cabin>();
		}
		return cabins;
	}
}