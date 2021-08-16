using HMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Web.ViewModels.Users
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
