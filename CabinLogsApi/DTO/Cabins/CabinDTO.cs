namespace CabinLogsApi.DTO.Cabins;

public class CabinDTO
{
    public int Id { get; set; }
    public DateTime created_at { get; set; }
    public string? Name { get; set; }
    public int MaxCapacity { get; set; }
    public decimal RegularPrice { get; set; }
    public decimal Discount { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
}
