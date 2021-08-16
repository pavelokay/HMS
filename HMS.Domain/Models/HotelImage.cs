using System.ComponentModel.DataAnnotations;

namespace HMS.Domain.Models
{
    public class HotelImage
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }


        [StringLength(100)]
        public string AltName { get; set; }
        public string ImageFile { get; set; }
    }
}
