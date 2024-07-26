using CabinLogsApi.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabinLogsApiTests.Fakes;

public class ApplicationDbContextFakeBuilder : IDisposable
{
    private readonly ApplicationDbContextFake _context = new();

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private EntityEntry<Cabin>? _basicCabin;
    private EntityEntry<Guest>? _guestOne;
    private EntityEntry<Setting>? _setting;
    private EntityEntry<Booking>? _bookingOne;

    public ApplicationDbContextFake Build()
    {
        _context.SaveChangesAsync();
        return _context;
    }

    public ApplicationDbContextFakeBuilder WithCabins()
    {
        _basicCabin = _context.Cabins.Add(new Cabin
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
        return this;
    }

    public ApplicationDbContextFakeBuilder WithGuests()
    {
        _guestOne = _context.Guests.Add(
            new Guest
            {
                id = 1,
                created_at = DateTime.UtcNow,
                fullName = "John Doe",
                nationalId = "dafdasfa",
                nationality = "American",
                countryFlag = null,
            }
        );
        return this;
    }

    public ApplicationDbContextFakeBuilder WithSettings()
    {
        _setting = _context.Settings.Add(
            new Setting
            {
                id = 1,
                created_at = DateTime.UtcNow,
                minBookingLength = 3,
                maxBookingLength = 90,
                breakfastPrice = 15,
            });

        return this;
    }

    public ApplicationDbContextFakeBuilder WithBookings()
    {
        _bookingOne = _context.Bookings.Add(new Booking
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

        return this;
    }
}

