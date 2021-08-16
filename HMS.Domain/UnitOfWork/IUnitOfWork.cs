using HMS.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace HMS.Domain.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        //void BeginTransaction();
        IUserRepository Users { get; }
        IClientRepository Clients { get; }
        IHotelComplexRepository HotelComplexes { get; }
        IHotelRepository Hotels { get; }
        IDepartmentRepository Departments { get; }
        IRoomRepository Rooms { get; }
        IRoomTypeRepository RoomTypes { get; }
        IReservationRepository Reservations { get; }
        ITransactionRepository Transactions { get; }
        IServiceRepository Services { get; }
        IPaymentRepository Payments { get; }
        Task SaveAsync();
    }
}
