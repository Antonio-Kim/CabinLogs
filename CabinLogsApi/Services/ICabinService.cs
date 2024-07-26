using CabinLogsApi.Models;

public interface ICabinService
{
	public Task<List<Cabin>> GetCabins();
	public Task<Cabin?> GetCabin(int id);
}