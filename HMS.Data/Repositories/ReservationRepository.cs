using HMS.Data.Repositories.Base;
using HMS.Domain;
using HMS.Domain.Models;
using HMS.Domain.Repositories;

namespace HMS.Data.Repositories
{
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        public ReservationRepository(HMSContext context) : base(context)
        { }


    }
}
