using System.Collections.Generic;
using System.Linq;
using FUMiniHotelSystem.Data.Database;
using FUMiniHotelSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FUMiniHotelSystem.Data.Repositories
{
    public class BookingRepository
    {
        public IEnumerable<Booking> GetAll()
        {
            using var context = DbContextFactory.CreateDbContext();
            return context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.BookingDetails)
                    .ThenInclude(d => d.Room)
                .AsNoTracking()
                .ToList();
        }

        public Booking? GetById(int id)
        {
            using var context = DbContextFactory.CreateDbContext();
            return context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.BookingDetails)
                    .ThenInclude(d => d.Room)
                .FirstOrDefault(b => b.BookingID == id);
        }

        public IEnumerable<Booking> GetByCustomer(int customerId)
        {
            using var context = DbContextFactory.CreateDbContext();
            return context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.BookingDetails)
                    .ThenInclude(d => d.Room)
                .Where(b => b.CustomerID == customerId)
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<Booking> GetByPeriod(DateTime start, DateTime end)
        {
            using var context = DbContextFactory.CreateDbContext();
            return context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.BookingDetails)
                    .ThenInclude(d => d.Room)
                .Where(b => b.BookingDetails.Any(d => d.StartDate >= start && d.EndDate <= end))
                .AsNoTracking()
                .ToList();
        }

        public void Add(Booking booking)
        {
            using var context = DbContextFactory.CreateDbContext();
            context.Bookings.Add(booking);
            context.SaveChanges();
        }
    }
}
