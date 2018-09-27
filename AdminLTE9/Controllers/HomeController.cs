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
            return View();
        }

        public ActionResult AnotherLink()
        {
            return View("Index");
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
            string a = Source;
            return View();
        }

        public bool IsFlightExist(string Source, string Destination, DateTime Date, string Class, int AdultNumber, int KidNumber, int LarvaNumber)
        {
            var check = db.Flights.Where(a => a.F_Origin == Source && a.F_Destination == Destination && a.F_Date == Date).FirstOrDefault();
            var checkCapacity = db.Airplanes.Where(b => b.A_Capacity == AdultNumber + KidNumber + LarvaNumber).FirstOrDefault();
            if (check != null && checkCapacity != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
