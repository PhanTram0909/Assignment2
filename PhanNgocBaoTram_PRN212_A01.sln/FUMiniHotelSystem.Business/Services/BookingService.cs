using System;
using System.Collections.Generic;
using FUMiniHotelSystem.Data.Models;
using FUMiniHotelSystem.Data.Repositories;

namespace FUMiniHotelSystem.Business.Services
{
    public class BookingService
    {
        private readonly BookingRepository _repo = new();

        public IEnumerable<Booking> GetAll() => _repo.GetAll();
        public Booking? GetById(int id) => _repo.GetById(id);
        public IEnumerable<Booking> GetByCustomer(int customerId) => _repo.GetByCustomer(customerId);
        public IEnumerable<Booking> GetByPeriod(DateTime start, DateTime end) => _repo.GetByPeriod(start, end);
        public void Create(Booking booking) => _repo.Add(booking);
    }
}
