using HMS.Data.Logging;
using HMS.Data.Repositories;
using HMS.Data.Repositories.Base;
using HMS.Data.UnitOfWork;
using HMS.Domain;
using HMS.Domain.Interfaces;
using HMS.Domain.Models;
using HMS.Domain.Repositories;
using HMS.Domain.Repositories.Base;
using HMS.Domain.UnitOfWork;
using HMS.Web.Services;
using HMS.Web.Services.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<HMSContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("HMS.Web")));

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<HMSContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
            });

            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.SlidingExpiration = true;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;

            });

            ConfigureAspnetRunServices(services);

            services.AddRazorPages()
                .AddRazorRuntimeCompilation();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Identity",
                    pattern: "{area:exists}/{controller=Account}/{action=Profile}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }


        private void ConfigureAspnetRunServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository> ();
            services.AddScoped<IHotelComplexRepository, HotelComplexRepository> ();
            services.AddScoped<IHotelRepository, HotelRepository> ();
            services.AddScoped<IDepartmentRepository, DepartmentRepository> ();
            services.AddScoped<IRoomTypeRepository, RoomTypeRepository> ();
            services.AddScoped<IRoomRepository, RoomRepository> ();
            services.AddScoped<ITransactionRepository, TransactionRepository> ();
            services.AddScoped<IReservationRepository, ReservationRepository> ();
            services.AddScoped<IServiceRepository, ServiceRepository> ();
            services.AddScoped<IPaymentRepository, PaymentRepository> ();
            services.AddScoped<IClientRepository, ClientRepository> ();
            services.AddScoped<IUnitOfWork, UnitOfWork> ();
            services.AddScoped<IAdminService, AdminService> ();
            services.AddScoped<IReservationService, ReservationService> ();

        }
    }
}
