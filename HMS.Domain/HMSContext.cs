using HMS.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace HMS.Domain
{
    public class HMSContext : IdentityDbContext<User, Role, string> 
    {
        public HMSContext(DbContextOptions<HMSContext> options)
            :base(options) { }
        //public DbSet<User> Users { get; set; }
        //public DbSet<Role> Roles { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<OrganizationClient> OrganizationClients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        
        public DbSet<HotelComplex> HotelComplexes { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<RoomStatus> RoomStatuses { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationType> ReservationTypes { get; set; }
        public DbSet<ReservationStatus> ReservationStatuses { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionStatus> TransactionStatuses { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<HotelComplexImage> HotelComplexImages { get; set; }
        public DbSet<HotelImage> HotelImages { get; set; }
        public DbSet<RoomTypeImage> RoomTypeImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //SetTableNamesAsSingle(builder);
            base.OnModelCreating(builder);

        }

        private static void SetTableNamesAsSingle(ModelBuilder builder)
        {
            // Use the entity name instead of the Context.DbSet<T> name
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                builder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name);
            }
        }
    }
}
