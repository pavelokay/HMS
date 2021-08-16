using System.ComponentModel.DataAnnotations;

namespace HMS.Domain.Models
{
    public class HotelComplexImage
    {
        public int Id { get; set; }
        public int HotelComplexId { get; set; }
        public HotelComplex HotelComplex { get; set; }


        [StringLength(100)]
        public string AltName { get; set; }
        public string ImageFile { get; set; }
    }
}
