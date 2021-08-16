using System;
using System.Threading.Tasks;
using HMS.Domain;
using HMS.Domain.Repositories;
using HMS.Domain.UnitOfWork;

namespace HMS.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HMSContext _context;
        public IUserRepository Users { get; }
        public IHotelComplexRepository HotelComplexes { get; }
        public IHotelRepository Hotels { get; }
        public IDepartmentRepository Departments { get; }
        public IRoomTypeRepository RoomTypes { get; }
        public IRoomRepository Rooms { get; }
        public IServiceRepository Services { get; }
        public ITransactionRepository Transactions { get; }
        public IReservationRepository Reservations { get; }
        public IPaymentRepository Payments { get; }
        public IClientRepository Clients { get; }

        public UnitOfWork(HMSContext context,
             IUserRepository usersRepository,
             IHotelComplexRepository hotelComplexesRepository,
             IHotelRepository hotelsRepository,
             IDepartmentRepository departmentsRepository,
             IRoomTypeRepository roomTypesRepository,
             IRoomRepository roomsRepository,
             IServiceRepository servicesRepository,
             ITransactionRepository transactionsRepository,
             IReservationRepository reservationsRepository,
             IPaymentRepository paymentsRepository,
             IClientRepository clientsRepository)
        {
            this._context = context;
            this.Users = usersRepository;
            this.HotelComplexes = hotelComplexesRepository;
            this.Hotels = hotelsRepository;
            this.RoomTypes = roomTypesRepository;
            this.Rooms = roomsRepository;
            this.Reservations = reservationsRepository;
            this.Transactions = transactionsRepository;
            this.Payments = paymentsRepository;
            this.Services = servicesRepository;
            this.Departments = departmentsRepository;
            this.Clients = clientsRepository;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual async Task DisposeAsync(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    await _context.DisposeAsync();
                }
            }
            this.disposed = true;
        }

        public async Task DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                     _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
