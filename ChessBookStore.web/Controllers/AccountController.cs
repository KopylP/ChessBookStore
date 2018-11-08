using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChessBookStore.web.Data;
using ChessBookStore.web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChessBookStore.web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        [HttpGet]
        public IActionResult Register() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        async public Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                User user = new User() { Name = model.Name, LastName = model.LastName, Email = model.Email, UserName = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if(result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    await _userManager.AddToRoleAsync(user, "USER");
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
            return View(model);
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null) => View(new LoginViewModel() { ReturnUrl = returnUrl});
        [HttpPost]
        [ValidateAntiForgeryToken]
        async public Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if(result.Succeeded)
                {
                    if(String.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Password or login are inccorect!");
                }
            }
            return View(model);
        }
        //[Authorize(Roles = "User")]
        [HttpGet]
        async public Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UserInformation()
        {
            User user =  await _userManager.Users.Include(p => p.Addresses).FirstOrDefaultAsync(p => p.Email == User.Identity.Name); //FindByEmailAsync(User.Identity.Name);
            EditUserViewModel model = new EditUserViewModel() { Name = user.Name, LastName = user.LastName, Email = user.Email, Addresses = user.Addresses };
            return View(model);
        }
        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult AddAddress(string userEmail)
        {
            var user = _userManager.Users.FirstOrDefault(p => p.Email == userEmail);
            if (User.Identity.Name == user.UserName)
            {
                if (user != null)
                {
                    Address address = new Address() { UserId = user.Id };
                    return View(address);
                }
            }
            return Content("Access Denied");
        }
        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AddAddress(Address address)
        {
            if(ModelState.IsValid)
            {
                _context.Addresses.Add(address);
                _context.SaveChanges();
                return RedirectToAction("UserInformation");
            }
            return View(address);
        }
        [Authorize(Roles = "User")]
        //[ValidateAntiForgeryToken]
        [HttpGet]
        public IActionResult EditAddress(int addressId)
        {
            var user = _userManager.Users.Include(p => p.Addresses).FirstOrDefault(p => p.UserName == User.Identity.Name);
            if (user != null)
            {
                var address = user.Addresses.FirstOrDefault(p => p.Id == addressId);
                if(address != null)
                return View(address);
            }
            return Content("Access Denied");
        }
        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult EditAddress(Address address)
        {
            if (ModelState.IsValid)
            {
                _context.Addresses.Update(address);
                _context.SaveChanges();
                return RedirectToAction("UserInformation");
            }
            return View(address);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet]
        public IActionResult AddressDelete(int addressId)
        {
            var user = _userManager.Users.Include(c => c.Addresses).FirstOrDefault(p => p.UserName == User.Identity.Name);
            if(user != null)
            {
                var address = user.Addresses.FirstOrDefault(p => p.Id == addressId);
                if(address != null)
                {
                    _context.Addresses.Remove(address);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("UserInformation");
        }
    }
}