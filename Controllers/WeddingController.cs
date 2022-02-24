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
    public class WeddingController : Controller
    {
        private int? uid
        {
            get
            {
                return HttpContext.Session.GetInt32("UserId");
            }
        }

        private bool loggedIn
        {
            get
            {
                return uid != null;
            }
        }
        private WeddingPlannerContext db;
        public WeddingController(WeddingPlannerContext context)
        {
            db = context;
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            if (!loggedIn)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Weddings = db.Weddings
                .Include(w => w.WeddingAttendees)
                .Include(w => w.Author)
                .ToList();

            return View("Dashboard");
        }

        [HttpPost("delete/{weddingId}")]
        public IActionResult DeleteWedding(int weddingId)
        {
            Wedding wedding = db.Weddings.FirstOrDefault(w => w.WeddingId == weddingId);
            if(wedding != null)
            {
                db.Weddings.Remove(wedding);
                db.SaveChanges();
            }
            return RedirectToAction("Dashboard");
        }

        [HttpPost("RSVP/{weddingId}")]
        public IActionResult RSVP(int weddingId)
        {
            if(!loggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            Attendance existingRSVP = db.Attendances
                .FirstOrDefault(RSVP => RSVP.WeddingId == weddingId && (int)uid == RSVP.UserId);

            if(existingRSVP == null)
            {
                Attendance rsvp = new Attendance()
                {
                    WeddingId = weddingId,
                    UserId = (int)uid
                };
                db.Attendances.Add(rsvp);
            }
            else 
            {
                db.Attendances.Remove(existingRSVP);
            }
            db.SaveChanges();
            return RedirectToAction("Dashboard");
        }


        [HttpGet("NewWedding")]
        public IActionResult NewWedding()
        {
            if(!loggedIn)
            {
                return RedirectToAction("Index");
            }
            return View("NewWedding");
        }

        [HttpPost("CreateWedding")]
        public IActionResult Create(Wedding newWedding)
        {
            if(ModelState.IsValid == false)
            {
                return View("NewWedding");
            }
            newWedding.UserId = (int)uid;
            db.Add(newWedding);
            db.SaveChanges();
            return RedirectToAction("Wedding");
            
        }
        
        [HttpGet("Wedding/{weddingId}")]
        public IActionResult Wedding(int weddingId)
        {

            Wedding wedding = db.Weddings
                .Include(p => p.Author)
                .Include(p => p.WeddingAttendees)
                .ThenInclude(rsvp => rsvp.User)
                .FirstOrDefault(w => w.WeddingId == weddingId);
            if(wedding == null)
            {
                return RedirectToAction("Dashboard");
            }
            return View("Wedding", wedding);
        }
    }
}