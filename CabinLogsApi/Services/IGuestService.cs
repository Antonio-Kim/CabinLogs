using CabinLogsApi.Models;

public interface IGuestService
{
	public List<Guest> GetGuests();
	public Guest GetGuest(int id);
}