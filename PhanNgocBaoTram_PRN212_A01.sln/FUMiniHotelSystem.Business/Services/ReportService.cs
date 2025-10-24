using System;
using System.Collections.Generic;
using System.Linq;
using FUMiniHotelSystem.Data.Database;
using FUMiniHotelSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FUMiniHotelSystem.Business.Services
{
    public class ReportService
    {
        public IEnumerable<object> GetRevenueByCustomer(DateTime start, DateTime end)
        {
            using var _db = DbContextFactory.CreateDbContext();

            // Lấy tất cả booking có booking details trong khoảng thời gian
            var bookings = _db.Bookings
                .Include(b => b.Customer)
                .Include(b => b.BookingDetails)
                .ThenInclude(d => d.Room)
                .AsNoTracking()
                .ToList();

            // Flatten BookingDetails trong khoảng thời gian
            var detailsInPeriod = bookings
                .SelectMany(b => b.BookingDetails
                    .Where(d => d.StartDate >= start && d.EndDate <= end)
                    .Select(d => new
                    {
                        CustomerID = b.CustomerID,
                        CustomerName = b.Customer?.CustomerFullName ?? "Unknown",
                        ActualPrice = d.ActualPrice
                    }))
                .ToList();

            // Group theo khách hàng và tính tổng
            var revenue = detailsInPeriod
                .GroupBy(d => d.CustomerID)
                .Select(g => new
                {
                    CustomerName = g.First().CustomerName,
                    Total = g.Sum(d => d.ActualPrice)
                })
                .OrderByDescending(x => x.Total)
                .ToList();

            return revenue;
        }
    }
}
