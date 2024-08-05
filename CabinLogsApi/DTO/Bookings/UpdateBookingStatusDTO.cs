namespace CabinLogsApi.DTO.Bookings;
public class UpdateBookingStatusDTO
{
    public string Status { get; set; } = "unconfirmed";
    public bool IsPaid { get; set; } = false;
}

