using System;
using System.Collections.Generic;
using System.Linq;
using FUMiniHotelSystem.Data.Database;
using FUMiniHotelSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FUMiniHotelSystem.Data.Repositories
{
    public class RoomRepository
    {
        public IEnumerable<Room> GetAll()
        {
            using var context = DbContextFactory.CreateDbContext();
            return context.Rooms
                .Include(r => r.RoomType)
                .Where(r => r.RoomStatus == 1)
                .AsNoTracking()
                .ToList();
        }

        public Room? GetById(int id)
        {
            using var context = DbContextFactory.CreateDbContext();
            return context.Rooms
                .Include(r => r.RoomType)
                .AsNoTracking()
                .FirstOrDefault(r => r.RoomID == id);
        }

        public void Add(Room room)
        {
            using var context = DbContextFactory.CreateDbContext();
            context.Rooms.Add(room);
            context.SaveChanges();
        }

        public void Update(Room room)
        {
            using var context = DbContextFactory.CreateDbContext();
            var existing = context.Rooms.FirstOrDefault(r => r.RoomID == room.RoomID);
            if (existing == null) throw new Exception("Room not found");

            existing.RoomNumber = room.RoomNumber;
            existing.RoomDescription = room.RoomDescription;
            existing.RoomMaxCapacity = room.RoomMaxCapacity;
            existing.RoomPricePerDate = room.RoomPricePerDate;
            existing.RoomTypeID = room.RoomTypeID;
            existing.RoomStatus = room.RoomStatus;

            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = DbContextFactory.CreateDbContext();
            var existing = context.Rooms.FirstOrDefault(r => r.RoomID == id);
            if (existing != null)
            {
                existing.RoomStatus = 2; // soft delete
                context.SaveChanges();
            }
        }

        public IEnumerable<Room> SearchByNumber(string roomNumber)
        {
            if (string.IsNullOrWhiteSpace(roomNumber)) return GetAll();
            return GetAll().Where(r => r.RoomNumber.Contains(roomNumber, StringComparison.OrdinalIgnoreCase));
        }
    }
}
