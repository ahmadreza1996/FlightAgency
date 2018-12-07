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
        private FlightAgencyEntities1 db = new FlightAgencyEntities1();
        public bool LoginStaus;

        // Home index Views.
        public ActionResult Index()
        {
            List<FlightClass> FlightClassList = new List<FlightClass>();
            List<InternalCity> InternalCityList = new List<InternalCity>();
            List<ExternalCity> ExternalCityList = new List<ExternalCity>();

            foreach (var FlightClass in db.FlightClasses)
            {
                FlightClassList.Add(FlightClass);
            }
            foreach (var InternalCity in db.InternalCities)
            {
                InternalCityList.Add(InternalCity);
            }
            foreach (var ExternalCity in db.ExternalCities)
            {
                ExternalCityList.Add(ExternalCity);
            }
            ViewBag.FlightClass = FlightClassList;
            ViewBag.InternalCity = InternalCityList;
            ViewBag.ExternalCity = ExternalCityList;
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
                        return RedirectToAction("Index", "Portal", new { TicketID = v2.P_TicketID });
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

        // Views for Passengers Portal for discover Information.
        public ActionResult Portal(string Source, string Destination, DateTime Date, string Class, int AdultNumber, int KidNumber, int LarvaNumber)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Portal()
        {
            return View();
        }

        // Show the AboutUs content to the User
        public ActionResult AboutUs()
        {
            return View();
        }

        // Views for search Flights for Passenger
        public ActionResult SearchFlights()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SearchFlights(string Source, string Destination, string Date, string Class, int AdultNumber, int KidNumber, int LarvaNumber)
        {
            List<Flight> SearchedFlightsList = new List<Flight>();

            var checkFlightExist = db.Flights.Where(a => a.F_Origin == Source && a.F_Destination == Destination && a.F_Date == Date && (Class != "همه" ? a.F_Class == Class : true) && a.F_Capacity > AdultNumber + KidNumber + LarvaNumber).FirstOrDefault();
            //var checkFlightExist = db.Flights.Where(a => a.F_Class == Class).FirstOrDefault();


            //var checkFlightExist = db.Flights.Where(a => a.F_Origin == Source && a.F_Destination == Destination && a.F_Date == Date && a.F_Capacity > AdultNumber + KidNumber + LarvaNumber).FirstOrDefault();

            if (checkFlightExist != null)
            {
                foreach (var flight in db.Flights)
                {
                    if (flight.F_Origin == Source
                        && flight.F_Destination == Destination
                        && flight.F_Date == Date && (Class != "همه" ? flight.F_Class == Class : true) && flight.F_Capacity > AdultNumber + KidNumber + LarvaNumber)
                    {
                        SearchedFlightsList.Add(flight);
                    }
                }
                ViewBag.SearchedFlights = SearchedFlightsList;
                ViewBag.AdultNumber = AdultNumber;
                ViewBag.KidNumber = KidNumber;
                ViewBag.LarvaNumber = LarvaNumber;
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

        // GET: Admin/Details/5
        public ActionResult AddPassengerToFlight(int? id, int? adult, int? kid, int? larva)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            foreach (var flight in db.Flights)
            {
                if (flight.F_ID == id)
                {
                    ViewBag.ID = id;
                    ViewBag.Origin = flight.F_Origin;
                    ViewBag.Destination = flight.F_Destination;
                    ViewBag.Date = flight.F_Date;
                    ViewBag.Time = flight.F_Time;
                    ViewBag.FlightClass = flight.F_Class;
                    ViewBag.AdultPrice = flight.F_AdultPrice;
                    ViewBag.KidPrice = flight.F_KidPrice;
                    ViewBag.LarvaPrice = flight.F_LarvaPrice;
                    ViewBag.AdultNumber = adult;
                    ViewBag.KidNumber = kid;
                    ViewBag.LarvaNumber = larva;
                    ViewBag.SumPrice = adult * flight.F_AdultPrice + kid * flight.F_KidPrice + larva * flight.F_LarvaPrice;
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddPassengerToFlight()
        {
            return View();
        }
        public ActionResult SaveAndSentEmail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveAndSendEmail(string FirstNameAdult1, AdminLTE9.Models.Airplane airplane)
        {
            string s = airplane.A_Name;
            return View();
        }
    }
}
