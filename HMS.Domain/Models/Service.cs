using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HMS.Domain.Models
{
    public class Service
    {
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Hotel> Hotels { get; set; }
    }
}
