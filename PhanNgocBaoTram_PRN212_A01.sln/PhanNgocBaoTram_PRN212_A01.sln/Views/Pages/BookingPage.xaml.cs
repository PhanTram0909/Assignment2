using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FUMiniHotelSystem.Business.Services;
using FUMiniHotelSystem.Data.Models;
using FUMiniHotelSystem.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace PhanNgocBaoTramWPF.Views.Pages
{
    public partial class BookingPage : Page
    {
        private readonly BookingService _bookingService = new();
        private readonly CustomerService _customerService = new();
        private readonly RoomService _roomService = new();

        public BookingPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using var context = DbContextFactory.CreateDbContext();

            // Load combobox
            cbCustomer.ItemsSource = context.Customers.Where(c => c.CustomerStatus == 1).ToList();
            cbRoom.ItemsSource = context.Rooms.Where(r => r.RoomStatus == 1).ToList();

            // Load bookings
            var bookings = context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.BookingDetails)
                    .ThenInclude(d => d.Room)
                .AsNoTracking()
                .ToList();

            // Flatten booking details cho DataGrid
            dgBookings.ItemsSource = bookings
                .SelectMany(b => b.BookingDetails.Select(d => new
                {
                    b.BookingID,
                    CustomerName = b.Customer?.CustomerFullName ?? "N/A",
                    RoomNumber = d.Room?.RoomNumber ?? "N/A",
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    TotalPrice = d.ActualPrice ?? 0
                }))
                .ToList();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (cbCustomer.SelectedItem is not Customer customer)
            {
                MessageBox.Show("Please select a customer.");
                return;
            }

            if (cbRoom.SelectedItem is not Room room)
            {
                MessageBox.Show("Please select a room.");
                return;
            }

            if (dpFrom.SelectedDate == null || dpTo.SelectedDate == null)
            {
                MessageBox.Show("Please select booking dates.");
                return;
            }

            DateTime start = dpFrom.SelectedDate.Value;
            DateTime end = dpTo.SelectedDate.Value;

            if (end <= start)
            {
                MessageBox.Show("End date must be after start date.");
                return;
            }

            decimal total;
            using (var context = DbContextFactory.CreateDbContext())
            {
                var roomEntity = context.Rooms.FirstOrDefault(r => r.RoomID == room.RoomID);
                if (roomEntity == null)
                {
                    MessageBox.Show("Room not found.");
                    return;
                }
                total = roomEntity.RoomPricePerDate * (end - start).Days;

            }

            var booking = new Booking
            {
                CustomerID = customer.CustomerID,
                BookingDate = DateTime.Now,
                TotalPrice = total,
                BookingDetails = new List<BookingDetail>
                {
                    new BookingDetail
                    {
                        RoomID = room.RoomID,
                        StartDate = start,
                        EndDate = end,
                        ActualPrice = total
                    }
                }
            };

            _bookingService.Create(booking);

            MessageBox.Show("Booking created successfully!");
            LoadData();
        }
    }
}
