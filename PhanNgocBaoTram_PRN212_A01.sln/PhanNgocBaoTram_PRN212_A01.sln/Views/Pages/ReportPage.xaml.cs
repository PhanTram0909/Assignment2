using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FUMiniHotelSystem.Business.Services;
using FUMiniHotelSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace PhanNgocBaoTramWPF.Views.Pages
{
    public partial class ReportPage : Page
    {
        private readonly BookingService _bookingService = new();

        public ReportPage()
        {
            InitializeComponent();
            dpStart.SelectedDate = DateTime.Today.AddMonths(-1);
            dpEnd.SelectedDate = DateTime.Today;
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (!dpStart.SelectedDate.HasValue || !dpEnd.SelectedDate.HasValue)
            {
                MessageBox.Show("Vui lòng chọn khoảng thời gian");
                return;
            }

            var start = dpStart.SelectedDate.Value;
            var end = dpEnd.SelectedDate.Value;

            if (end < start)
            {
                MessageBox.Show("Ngày kết thúc phải sau ngày bắt đầu");
                return;
            }

            try
            {
                // Lấy bookings có include BookingDetails và Customer
                var bookings = _bookingService.GetByPeriod(start, end);

                // Flatten BookingDetails cho DataGrid
                var bookingData = bookings
                    .SelectMany(b => b.BookingDetails.Select(d => new
                    {
                        b.BookingID,
                        CustomerName = b.Customer?.CustomerFullName ?? "N/A",
                        RoomNumber = d.Room?.RoomNumber ?? "N/A",
                        StartDate = d.StartDate,
                        EndDate = d.EndDate,
                        Days = (d.EndDate - d.StartDate).Days,
                        TotalPrice = d.ActualPrice
                    }))
                    .OrderByDescending(x => x.TotalPrice)
                    .ToList();

                dgReport.ItemsSource = bookingData;

                // Tổng quan
                var totalBookings = bookingData.Count;
                var totalRevenue = bookingData.Sum(b => b.TotalPrice);
                var averageRevenue = totalBookings > 0 ? totalRevenue / totalBookings : 0;

                txtTotalBookings.Text = $"Tổng số booking: {totalBookings}";
                txtTotalRevenue.Text = $"Tổng doanh thu: {totalRevenue:C0}";
                txtAverageRevenue.Text = $"Doanh thu trung bình: {averageRevenue:C0}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo báo cáo: {ex.Message}");
            }
        }
    }
}
