using Microsoft.EntityFrameworkCore;
using FUMiniHotelSystem.Data.Models;

namespace FUMiniHotelSystem.Data.Database
{
    public class FUMiniHotelDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingDetail> BookingDetails { get; set; }

        public FUMiniHotelDbContext(DbContextOptions<FUMiniHotelDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Room>().ToTable("RoomInformation");
            modelBuilder.Entity<RoomType>().ToTable("RoomType");
            modelBuilder.Entity<Booking>().ToTable("BookingReservation");

            // Map BookingDetail và khai báo composite key
            modelBuilder.Entity<BookingDetail>()
                .ToTable("BookingDetail")
                .HasKey(bd => new { bd.BookingReservationID, bd.RoomID });

            // Quan hệ BookingDetail -> Booking
            modelBuilder.Entity<BookingDetail>()
                .HasOne(bd => bd.Booking)
                .WithMany(b => b.BookingDetails)
                .HasForeignKey(bd => bd.BookingReservationID);

            // Quan hệ BookingDetail -> Room
            modelBuilder.Entity<BookingDetail>()
                .HasOne(bd => bd.Room)
                .WithMany()
                .HasForeignKey(bd => bd.RoomID);
        }
    }
}
