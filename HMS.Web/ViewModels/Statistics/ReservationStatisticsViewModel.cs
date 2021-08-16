using HMS.Domain.Models;
using System.Collections.Generic;

namespace HMS.Web.ViewModels.Statistics
{
    public class ReservationStatisticsViewModel
    {
        public IList<Reservation> Reservations { get; set; }
        public IList<User> Users { get; set; }
        public int? ModeId { get; set; }
        public string UserLogin { get; set; }
        public string DateRange { get; set; }
        public string FullPeriod { get; set; }
    }
}
