using CabinLogsApi.Models;

interface IBookingService
{
	public Task<List<Booking>> GetBookings();
	public Task<Booking> GetBooking(int id);
}