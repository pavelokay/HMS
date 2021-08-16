using HMS.Data.Repositories.Base;
using HMS.Domain;
using HMS.Domain.Models;
using HMS.Domain.Repositories;


namespace HMS.Data.Repositories
{
    public class HotelRepository : Repository<Hotel>, IHotelRepository 
    {
        public HotelRepository(HMSContext context) : base(context)
        { }


    }
}
