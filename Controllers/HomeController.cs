using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wedding_Planner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Wedding_Planner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MyContext _context;

        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            _logger = logger;
            _context = context;
        }





        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return View();
        }




        [HttpPost("user/create")]
        public IActionResult CreateUser(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.Any(l => l.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                _context.Add(newUser);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                return RedirectToAction("Dashboard");
            } else {
                return View("Index");
            }
        }





        [HttpPost("user/login")]
        public IActionResult LoginUser(Login_User LoggedIn)
        {
            if(ModelState.IsValid)
            {
                User userInDb = _context.Users.FirstOrDefault(l => l.Email == LoggedIn.Login_Email);
                if(userInDb == null)
                {
                    ModelState.AddModelError("Login_Email", "Invalid Email or Password");
                    return View("Index");
                }
                PasswordHasher<Login_User> hasher = new PasswordHasher<Login_User>();

                var result = hasher.VerifyHashedPassword(LoggedIn, userInDb.Password, LoggedIn.Login_Password);

                if (result == 0)
                {
                    ModelState.AddModelError("Login_Eamil", "Invalid Email or Password!");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                return RedirectToAction("Dashboard");
            }
            else 
            {
                return View("Index");
            }
        }
            




        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
             if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Login_User = _context.Users.FirstOrDefault(l => l.UserId == HttpContext.Session.GetInt32("UserId"));
            ViewBag.AllWeddings = _context.Weddings.Include(l => l.Users_Already_RSVP).OrderBy(m => m.Date).ToList();
            return View();
        }

        
        [HttpGet("/add/wedding")]
        public IActionResult AddWedding()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            return View("Create_Wedding");
        }




        [HttpPost("create/wedding")]
        public IActionResult CreateWedding(Wedding newWedding)
        {
            if(ModelState.IsValid)
            {
                newWedding.UserId = (int)HttpContext.Session.GetInt32("UserId");
                _context.Weddings.Add(newWedding);
                _context.SaveChanges();
                return RedirectToAction("ViewWedding", new {WeddingId = newWedding.WeddingId});
            } else {
                return View("Create_Wedding");
            }
        }


        
        [HttpGet("view/{WeddingId}")]
        public IActionResult ViewWedding(int WeddingId)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("View_Wedding");
            }
            ViewBag.AllWeddings = _context.Weddings.FirstOrDefault(l => l.WeddingId == WeddingId);
            List<Guest> Guests = _context.Guests.Where(l => l.WeddingId == WeddingId).Include(l => l.User).ToList();
            ViewBag.Guests = Guests;
            return View("View_Wedding");
        }
        
        
        [HttpGet("rsvp/{WeddingId}")]
        public IActionResult rsvp(int WeddingId)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            _context.Guests.Add(new Guest{UserId = (int)HttpContext.Session.GetInt32("UserId"), WeddingId = WeddingId});
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }



        [HttpGet("unrsvp/{WeddingId}")]
        public IActionResult unrsvp(int WeddingId)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            var Guest = _context.Guests.FirstOrDefault(l => l.UserId == (int)HttpContext.Session.GetInt32("UserId") && l.WeddingId == WeddingId);
            _context.Guests.Remove(Guest);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        

        [HttpGet("delete/{WeddingId}")]
        public IActionResult Delete(int WeddingId)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            var Wedding = _context.Weddings.FirstOrDefault(l => l.WeddingId == WeddingId);
            _context.Weddings.Remove(Wedding);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        
        
        
        
        
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
