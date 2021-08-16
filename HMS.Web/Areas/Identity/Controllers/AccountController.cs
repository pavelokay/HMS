using HMS.Domain.Models;
using HMS.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HMS.Web.ViewModels.Users;
using HMS.Domain.UnitOfWork;
using Microsoft.Extensions.Logging;
using HMS.Web.Controllers;

namespace HMS.Web.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private IUnitOfWork unitOfWork;
        private IPasswordHasher<User> passwordHasher;
        private readonly ILogger<User> logger;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUnitOfWork unitOfWork,
            IPasswordHasher<User> passwordHasher,
            ILogger<User> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.unitOfWork = unitOfWork;
            this.passwordHasher = passwordHasher;
            this.logger = logger;
        }
        // GET: Account/Register
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]

        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userDB = await unitOfWork.Users.GetAsync(u => u.UserName == model.UserName || u.Email == model.Email);
                if (!userDB.Any())
                {
                    var user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.UserName,
                        PhoneNumber = model.PhoneNumber,
                        GenderId = model.GenderId,
                        Email = model.Email,
                        Country = model.Country,
                        Town = model.Town,
                        Address = model.Address,
                        CreatedAt = DateTime.Now.Date
                    };
                    var result = await userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        logger.LogInformation("User created a new account with password.");
                        var role = "client";
                        await signInManager.SignInAsync(user, false);
                        await userManager.AddToRoleAsync(user, role);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Такой логин и (или) email уже зарегестрирован");
                }
            }
            return View(model);
        }
        // GET: Account/Login
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    logger.LogInformation("User logged in.");
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неверный логин и (или) пароль");
                }
            }
            return View(model);
        }
        // POST: Account/Logout
        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await signInManager.SignOutAsync();
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // GET: Account/Profile
        public async Task<IActionResult> Profile()
        {
            var userName = User.Identity.Name;
            var user = await userManager.FindByNameAsync(userName);

            var userTransactionsDB = await unitOfWork.Transactions.GetAsync(
                t => t.ClientId == user.Id,
                include: tr => tr
                .Include(trans => trans.Services)
                .Include(trans => trans.Reservation).ThenInclude(reserv => reserv.Rooms).ThenInclude(room => room.RoomType).ThenInclude(rt => rt.Hotel)
                .Include(trans => trans.TransactionStatus),
                disableTracking: true);

            var userTransactions = userTransactionsDB.ToList();
            var userReservations = new List<Reservation>();
            foreach (var transaction in userTransactions.Where(trans => trans.Services.Count == 0))
            {
                userReservations.Add(transaction.Reservation);
            }

            return View(new ProfileViewModel { User = user, Reservations = userReservations, Transactions = userTransactions });
        }

        // GET: Account/EditProfile/{id}
        public async Task<IActionResult> EditProfile(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }
            var userDB = await userManager.FindByIdAsync(id);
            if (userDB == null)
            {
                return NotFound();
            }
            var editVM = new EditProfileViewModel
            {
                UserName = userDB.UserName,
                FirstName = userDB.FirstName,
                LastName = userDB.LastName,
                Email = userDB.Email,
                PhoneNumber = userDB.PhoneNumber,
                Country = userDB.Country,
                Town = userDB.Town,
                Address = userDB.Address,
                Roles = await userManager.GetRolesAsync(userDB)
            };
            return View(editVM);
        }
        
        // POST: Account/EditProfile/{id}
        [HttpPost, ActionName("EditProfile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfilePost(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ModelState.AddModelError("", $"User with Id = { model.Id} cannot be found");
                return NotFound();
            }
            else
            {
                user.UserName = model.UserName;
                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    if (user.PasswordHash == passwordHasher.HashPassword(user, model.OldPassword)){
                        user.PasswordHash = passwordHasher.HashPassword(user, model.NewPassword);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Old password is incorrect");
                        return View(model);
                    }
                }
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Country = model.Country;
                user.Town = model.Town;
                user.Address = model.Address;
            }
            
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Profile));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        // GET: Account/Delete/{id}
        public async Task<IActionResult> DeleteProfile(string id, bool? saveChangesError = false)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }
            return View(user);
        }
        

        // POST: Account/Delete/{id}
        [HttpPost, ActionName("DeleteProfile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProfileConfirmed(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(id);
        }
    }
}
