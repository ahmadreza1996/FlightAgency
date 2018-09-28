using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminLTE9.Models;

namespace AdminLTE9.Controllers
{
    public class HomeController : Controller
    {
        private FlightAgencyEntities db = new FlightAgencyEntities();

        public ActionResult Index()
        {
            List<FlightClass> FlightClassList = db.FlightClasses.ToList();
            ViewBag.FlightClass = FlightClassList;
            return View();
        }

        public ActionResult Portal()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Portal([Bind(Include = "P_IdentityCode,P_Password")] Passenger passenger)
        {
            // Model Validation.
            if (ModelState.IsValid)
            {
                // Password Hashing for more Security
                passenger.P_Password = Crypto.Hash(passenger.P_Password);

                //Check Passenger Exist
                var isExist = IsPassengerExist(passenger.P_IdentityCode, passenger.P_Password);
                if (isExist)
                {
                    //// Use @Html.ValidationMessage("UserNameExist", new { @class = "text-danger"}) in View for show the mesage to user.
                    //ModelState.AddModelError("UsernameExist", "Username already Exist");
                    //ViewBag.UsernameExisted = "نام کابری وارد شده در سیستم وجود دارد";
                    //return View(passenger);
                    return View();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult Flights()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Flights(string Source, string Destination, DateTime Date, string Class, int AdultNumber, int KidNumber, int LarvaNumber)
        {
            var check = db.Flights.Where(a => a.F_Origin == Source && a.F_Destination == Destination && a.F_Date == Date).FirstOrDefault();
            var checkCapacity = db.Airplanes.Where(b => b.A_Capacity == AdultNumber + KidNumber + LarvaNumber).FirstOrDefault();
            if (check != null && checkCapacity != null)
            {
                return View();
            }
            else
            {
                return View();
            }
        }

        public bool IsPassengerExist(string IdentityCode, string Password)
        {
            var v = db.Passengers.Where(a => a.P_IdentityCode == IdentityCode && a.P_Password == Password).FirstOrDefault();
            return v != null;
        }
    }
}
