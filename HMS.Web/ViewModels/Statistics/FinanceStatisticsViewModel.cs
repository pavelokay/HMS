using HMS.Domain.Models;
using System.Collections.Generic;

namespace HMS.Web.ViewModels.Statistics
{
    public class FinanceStatisticsViewModel
    {
        public IList<Transaction> Transactions { get; set; }
        public string RoleName { get; set; }
        public string DateRange { get; set; }
        public string FullPeriod { get; set; }
    }
}
