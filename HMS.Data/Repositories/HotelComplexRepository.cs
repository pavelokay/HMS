using HMS.Data.Repositories.Base;
using HMS.Domain;
using HMS.Domain.Models;
using HMS.Domain.Repositories;
using System.Threading.Tasks;
using System.Linq;

namespace HMS.Data.Repositories
{
    public class HotelComplexRepository : Repository<HotelComplex>, IHotelComplexRepository
    {
        public HotelComplexRepository(HMSContext context) : base(context)
        { }

        //public async Task<HotelComplex> GetHotelComplexByNameAsync(string hotelComplexName)
        //{
        //    //return await GetAsync(h => h.Name.ToLower().Contains(hotelComplexName.ToLower()));
        //    return (await GetAsync(x => x.Name == hotelComplexName)).FirstOrDefault();
        //}
    }
}
