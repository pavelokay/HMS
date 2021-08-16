using HMS.Data.Repositories.Base;
using HMS.Domain;
using HMS.Domain.Models;
using HMS.Domain.Repositories;


namespace HMS.Data.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(HMSContext context) : base(context)
        { }


    }
}
