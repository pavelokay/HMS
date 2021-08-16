using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HMS.Domain.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Check In")]
        public DateTime CheckIn { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Check Out")]
        public DateTime CheckOut { get; set; }

        [Range(1, 10)]
        public int GuestCount { get; set; }
        [Range(1, 100)]
        public int RoomCount { get; set; }
        public int DaysDuration
        {
            get { return CheckOut.Subtract(CheckIn).Days; }
        }
        public int ReservationTypeId { get; set; }
        public ReservationType ReservationType { get; set; }
        public int ReservationStatusId { get; set; }
        public ReservationStatus ReservationStatus { get; set; }

        public ICollection<Room> Rooms { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
