using HMS.Data.Repositories.Base;
using HMS.Domain;
using HMS.Domain.Models;
using HMS.Domain.Repositories;

namespace HMS.Data.Repositories
{
    public class ClientRepository :  Repository<Client>, IClientRepository
    {
        public ClientRepository(HMSContext context) : base(context)
        { }
    }
}
