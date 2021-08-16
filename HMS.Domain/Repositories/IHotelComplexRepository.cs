using HMS.Domain.Models;
using HMS.Domain.Repositories.Base;
using System.Threading.Tasks;

namespace HMS.Domain.Repositories
{
    public interface IHotelComplexRepository : IRepository<HotelComplex>
    {
        //public Task<HotelComplex> GetHotelComplexByNameAsync(string hotelComplexName);
    }
}
