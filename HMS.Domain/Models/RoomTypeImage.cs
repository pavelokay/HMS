using System.ComponentModel.DataAnnotations;

namespace HMS.Domain.Models
{
    public class RoomTypeImage
    {
        public int Id { get; set; }
        public int RoomTypeId { get; set; }
        public RoomType RoomType { get; set; }


        [StringLength(100)]
        public string AltName { get; set; }
        public string ImageFile { get; set; }
    }
}
