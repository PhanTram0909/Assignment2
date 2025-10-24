using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUMiniHotelSystem.Data.Models
{
    [Table("BookingReservation")]
    public class Booking
    {
        [Column("BookingReservationID")]
        public int BookingID { get; set; }

        public DateTime BookingDate { get; set; }
        public decimal TotalPrice { get; set; }

        public int CustomerID { get; set; }
        public Customer Customer { get; set; } = null!; // tránh cảnh báo non-nullable

        public byte? BookingStatus { get; set; }

        public ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();
    }

    [Table("BookingDetail")]
    public class BookingDetail
    {
        // Composite key: BookingReservationID + RoomID
        public int BookingReservationID { get; set; }
        public Booking Booking { get; set; } = null!;

        public int RoomID { get; set; }
        public Room Room { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal? ActualPrice { get; set; }
    }
}
