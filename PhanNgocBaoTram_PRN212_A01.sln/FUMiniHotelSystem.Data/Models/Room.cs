using System.ComponentModel.DataAnnotations.Schema;

namespace FUMiniHotelSystem.Data.Models
{
    public class Room
    {
        public int RoomID { get; set; }

        public string RoomNumber { get; set; }

        [Column("RoomDetailDescription")]
        public string RoomDescription { get; set; }

        public int RoomMaxCapacity { get; set; }

        [Column("RoomPricePerDay")]
        public decimal RoomPricePerDate { get; set; }

        public int RoomTypeID { get; set; }
        public RoomType RoomType { get; set; }

        public byte RoomStatus { get; set; } // 1 = active, 2 = deleted
    }
}
