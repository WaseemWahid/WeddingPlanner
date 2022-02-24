using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private WeddingPlannerContext db;
        public HomeController(WeddingPlannerContext context)
        {
            db = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("Dashboard", "Wedding");
            }

            return View("Index");
        }

        [HttpPost("/register")]
        public IActionResult Register(User newUser)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "is taken");
                }
            }

            // Check if any errors existing originally OR were added manually from above.
            if (ModelState.IsValid == false)
            {
                // To display error messages on the form.
                return View("Index");
            }

            PasswordHasher<User> hasher = new PasswordHasher<User>();
            newUser.Password = hasher.HashPassword(newUser, newUser.Password);

            db.Users.Add(newUser);
            db.SaveChanges();

            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            HttpContext.Session.SetString("FullName", newUser.FullName());
            return RedirectToAction("Dashboard", "Wedding");
        }

        [HttpPost("/login")]
        public IActionResult Login(LoginUser loginUser)
        {
            if (ModelState.IsValid == false)
            {
                // Display errors on form.
                return View("Index");
            }

            User dbUser = db.Users
                .FirstOrDefault(u => u.Email == loginUser.LoginEmail);

            if (dbUser == null)
            {
                /* 
                In order to not reveal too much information (which one was wrong?)
                often generic messages are preferred, e.g., 'Incorrect credentials'.
                but for testing purposes we will be specific.
                */
                ModelState.AddModelError("LoginEmail", "Email not found.");
            }

            // If any error was added manually above.
            if (ModelState.IsValid == false)
            {
                return View("Index");
            }

            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
            PasswordVerificationResult pwCompareResult = hasher.VerifyHashedPassword(loginUser, dbUser.Password, loginUser.LoginPassword);

            if (pwCompareResult == 0)
            {
                ModelState.AddModelError("LoginEmail", "Incorrect password.");
                return View("Index");
            }

            HttpContext.Session.SetInt32("UserId", dbUser.UserId);
            HttpContext.Session.SetString("FullName", dbUser.FullName());
            return RedirectToAction("Dashboard", "Wedding");
        }

        [HttpPost("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}