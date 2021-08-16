using HMS.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace HMS.Domain
{
    public class HMSRoleInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var context = serviceProvider.GetRequiredService<HMSContext>();

            string adminUserName = "Admin";
            string adminPassword = "123456Qq!";

            string[] roleNames = { "admin", "accommodationOfficer", "accountant", "client", "organization" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new Role(roleName));
                }
            }


            if (await userManager.FindByNameAsync(adminUserName) == null)
            {
                var admin = new Employee
                {
                    FirstName = "Pavel",
                    LastName = "Kaysarov",
                    UserName = adminUserName,
                    PhoneNumber = "123456789",
                    Email = "adminHMS@gmail.com",
                    Country = "Russia",
                    Town = "Komsomolsk-on-Amur",
                    Address = "Lenina 27",
                    GenderId = 2,
                    CreatedAt = DateTime.Now.Date,
                    Department = await context.Departments.FindAsync(4) 
                };
                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }


            string accommodationOfficerUserName = "AccommodationOfficerHMS";
            string accommodationOfficerPassword = "123456Qq!";
            if (await userManager.FindByNameAsync(accommodationOfficerUserName) == null)
            {
                Employee accommodationOfficer = new Employee
                {
                    FirstName = "John",
                    LastName = "Veez",
                    UserName = accommodationOfficerUserName,
                    PhoneNumber = "987654321",
                    Email = "accommodationOfficerHMS@gmail.com",
                    Country = "Russia",
                    Town = "Komsomolsk-on-Amur",
                    Address = "Lenina 27",
                    GenderId = 2,
                    CreatedAt = DateTime.Now.Date,
                    Department = await context.Departments.FindAsync(3)
                };
                IdentityResult result = await userManager.CreateAsync(accommodationOfficer, accommodationOfficerPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(accommodationOfficer, "accommodationOfficer");
                }
            }

            string accountantUserName = "accountantHMS";
            string accountantPassword = "123456Qq!";
            if (await userManager.FindByNameAsync(accountantUserName) == null)
            {
                Employee accountant = new Employee
                {
                    FirstName = "Robin",
                    LastName = "Kras",
                    UserName = accountantUserName,
                    PhoneNumber = "123454321",
                    Email = "accountantHMS@gmail.com",
                    Country = "Russia",
                    Town = "Komsomolsk-on-Amur",
                    Address = "Lenina 27",
                    GenderId = 1,
                    CreatedAt = DateTime.Now.Date,
                    Department = await context.Departments.FindAsync(3)
                };
                IdentityResult result = await userManager.CreateAsync(accountant, accountantPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(accountant, "accountant");
                }
            }


            string testClientUserName = "testClientHMS";
            string testClientPassword = "123456Qq!";
            if (await userManager.FindByNameAsync(testClientUserName) == null)
            {
                Client testClient = new Client
                {
                    FirstName = "John",
                    LastName = "Doe",
                    UserName = testClientUserName,
                    PhoneNumber = "123123123",
                    Email = "testClientHMS@gmail.com",
                    Country = "Russia",
                    Town = "Komsomolsk-on-Amur",
                    Address = "Lenina 27",
                    GenderId = 2,
                    CreatedAt = DateTime.Now.Date
                };
                IdentityResult result = await userManager.CreateAsync(testClient, testClientPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(testClient, "client");
                }
            }


            string testOrganizationUserName = "testOrganizationHMS";
            string testOrganizationPassword = "123456Qq!";
            if (await userManager.FindByNameAsync(testOrganizationUserName) == null)
            {
                OrganizationClient testOrganization = new OrganizationClient
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    UserName = testOrganizationUserName,
                    PhoneNumber = "123456789",
                    Email = "testOrganizationHMS@gmail.com",
                    Country = "Russia",
                    Town = "Komsomolsk-on-Amur",
                    Address = "Lenina 27",
                    GenderId = 1,
                    CreatedAt = DateTime.Now.Date,
                    OrganizationName = "TestOrganization"
                };
                IdentityResult result = await userManager.CreateAsync(testOrganization, testOrganizationPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(testOrganization, "organization");
                }
            }
        }
    }
}
