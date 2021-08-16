using HMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Web.ViewModels.Statistics
{
    public class RoomTypeStatisticsViewModel
    {
        public IEnumerable<RoomType> RoomTypes{get;set;}
        public int? ModeId { get; set; }
    }
}
