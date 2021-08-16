using HMS.Data.Repositories.Base;
using HMS.Domain;
using HMS.Domain.Models;
using HMS.Domain.Repositories;


namespace HMS.Data.Repositories
{
    public class RoomTypeRepository : Repository<RoomType>, IRoomTypeRepository
    {
        public RoomTypeRepository(HMSContext context) : base(context)
        { }


    }
}
