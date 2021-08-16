using HMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Web.ViewModels.HotelController
{
    public class RoomTypeDetailsViewModel
    {
        public Hotel Hotel { get; set; }
        public RoomType RoomType { get; set; }
    }
}
