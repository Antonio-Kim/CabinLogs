using CabinLogsApi.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.Entity<Booking>()
			.HasKey(i => new { i.cabinId, i.guestId });
		modelBuilder.Entity<Booking>()
			.HasOne(c => c.Cabin)
			.WithMany(b => b.bookings)
			.HasForeignKey(k => k.cabinId)
			.IsRequired()
			.OnDelete(DeleteBehavior.Cascade);
		modelBuilder.Entity<Booking>()
			.HasOne(g => g.Guest)
			.WithMany(b => b.bookings)
			.HasForeignKey(k => k.guestId)
			.IsRequired()
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<Cabin>().HasData(new Cabin
		{
			id = 1,
			created_at = DateTime.UtcNow,
			name = "001",
			maxCapacity = 2,
			regularPrice = 250,
			discount = 50,
			description = "Small luxury cab in the woods",
			image = null,
		});
		modelBuilder.Entity<Cabin>().HasData(new Cabin
		{
			id = 2,
			created_at = DateTime.UtcNow,
			name = "002",
			maxCapacity = 4,
			regularPrice = 400,
			discount = 75,
			description = "test",
			image = null,
		});
		modelBuilder.Entity<Cabin>().HasData(new Cabin
		{
			id = 3,
			created_at = DateTime.UtcNow,
			name = "003",
			maxCapacity = 4,
			regularPrice = 400,
			discount = 0,
			description = null,
			image = null,
		});
		modelBuilder.Entity<Cabin>().HasData(new Cabin
		{
			id = 4,
			created_at = DateTime.UtcNow,
			name = "004",
			maxCapacity = 4,
			regularPrice = 400,
			discount = 0,
			description = null,
			image = null,
		});
		modelBuilder.Entity<Cabin>().HasData(new Cabin
		{
			id = 5,
			created_at = DateTime.UtcNow,
			name = "005",
			maxCapacity = 4,
			regularPrice = 800,
			discount = 0,
			description = null,
			image = null,
		});
		modelBuilder.Entity<Cabin>().HasData(new Cabin
		{
			id = 6,
			created_at = DateTime.UtcNow,
			name = "003",
			maxCapacity = 10,
			regularPrice = 1000,
			discount = 200,
			description = null,
			image = null,
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 1,
			created_at = DateTime.UtcNow,
			fullName = "John Doe",
			nationalId = "dafdasfa",
			nationality = "American",
			countryFlag = null,
		});
		modelBuilder.Entity<Setting>().HasData(new Setting
		{
			id = 1,
			created_at = DateTime.UtcNow,
			minBookingLength = 3,
			maxBookingLength = 90,
			breakfastPrice = 15,
		});
		modelBuilder.Entity<Booking>().HasData(new Booking
		{
			id = 1,
			created_at = DateTime.UtcNow,
			startDate = DateTime.Parse("2024-07-22 10:00:00"),
			endDate = DateTime.Parse("2024-07-27 17:00:00"),
			numberOfNights = 4,
			numGuests = 2,
			cabinPrice = 300,
			extrasPrice = 120,
			totalPrice = 420,
			status = "unconfirmed",
			hasBreakfast = true,
			isPaid = true,
			observations = "I will arrive at 10 am",
			cabinId = 1,
			guestId = 1,
		});
	}

	public DbSet<Cabin> Cabins => Set<Cabin>();
	public DbSet<Guest> Guests => Set<Guest>();
	public DbSet<Setting> Settings => Set<Setting>();
	public DbSet<Booking> Bookings => Set<Booking>();
}