using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HMS.Domain.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public Client Client { get; set; }
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }
        public int? PaymentId { get; set; }
        public Payment Payment { get; set; }
        public int TransactionStatusId { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }
        public ICollection<Service> Services { get; set; }

        [DataType(DataType.Currency)]
        public decimal Cost { get; set; }
    }
}
