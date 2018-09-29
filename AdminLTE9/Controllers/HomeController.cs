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
        public bool LoginStaus;

        public ActionResult Index()
        {
            List<FlightClass> FlightClassList = db.FlightClasses.ToList();
            ViewBag.FlightClass = FlightClassList;
            ViewBag.LoginStatus = LoginStaus;
            LoginStaus = false;
            return View();
        }

        // Handle the signing in to Portal.
        [HttpPost]
        public ActionResult Index([Bind(Include = "P_IdentityCode,P_Password")] Passenger passenger)
        {
            // Model Validation.
            if (ModelState.IsValid)
            {
                // Password Hashing for more Security
                //passenger.P_Password = Crypto.Hash(passenger.P_Password);

                //Check Passenger Exist
                var v1 = db.Passengers.Where(a => a.P_IdentityCode == passenger.P_IdentityCode).FirstOrDefault();
                if (v1 != null)
                {
                    var v2 = db.Passengers.Where(a => a.P_IdentityCode == passenger.P_IdentityCode && a.P_Password == passenger.P_Password).FirstOrDefault();
                    if (v2 != null)
                    {
                        return RedirectToAction("Portal", "Home");
                    }
                    ViewBag.LoginError = true;
                    ModelState.AddModelError("ValidationCodeWrong", "کد اعتبارسنجی وارد شده صحیح نمی باشد");
                    //return View(passenger);
                    return View();
                }
                ViewBag.LoginError = true;
                ModelState.AddModelError("ValidationCodeWrong", "کد ملی و یا کد اعتبارسنجی وارد شده صحیح نمی باشد");
                return View();
            }
            ViewBag.LoginError = true;
            return View();
        }

        public ActionResult Portal(string Source, string Destination, DateTime Date, string Class, int AdultNumber, int KidNumber, int LarvaNumber)
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult SearchFlights()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult SearchFlights(string Source, string Destination, DateTime Date, string Class, int AdultNumber, int KidNumber, int LarvaNumber)
        {
            List<Flight> SearchedFlightsList = new List<Flight>();

            var checkFlightExist = db.Flights.Where(a => a.F_Origin == Source && /*a.F_Destination == Destination && a.F_Date.ToString() == Date.ToString() &&*/ a.F_Capacity > AdultNumber + KidNumber + LarvaNumber).FirstOrDefault();
            if (checkFlightExist != null)
            {
                foreach (var flight in db.Flights)
                {
                    if (flight.F_Origin == Source
                        && flight.F_Destination == Destination
                        && flight.F_Capacity > AdultNumber + KidNumber + LarvaNumber)
                    {
                        SearchedFlightsList.Add(flight);
                    }
                }
                ViewBag.SearchedFlights = SearchedFlightsList;
                return View();
            }
            else
            {
                return View();
            }
            



            //var check = db.Flights.Where(a => a.F_Origin == Source && a.F_Destination == Destination && a.F_Date.ToString() == Date.ToString() && a.F_Capacity > AdultNumber + KidNumber + LarvaNumber).FirstOrDefault();
            //if (check != null)
            //{
            //    return RedirectToAction("Portal", new { Source, Destination, Date, Class, AdultNumber, KidNumber, LarvaNumber });
            //}
            //else
            //{
            //    return View("Index");
            //}
        }

        public bool IsPassengerExist(string IdentityCode, string Password)
        {
            var v = db.Passengers.Where(a => a.P_IdentityCode == IdentityCode && a.P_Password == Password).FirstOrDefault();
            return v != null;
        }
    }
}
