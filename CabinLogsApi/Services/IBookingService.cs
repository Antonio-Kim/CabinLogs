using CabinLogsApi.Models;

public interface IBookingService
{
	public Task<List<Booking>> GetBookings();
	public Task<Booking?> GetBooking(int id);
}