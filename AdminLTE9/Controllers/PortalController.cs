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
    public class PortalController : Controller
    {
        private FlightAgencyEntities1 db = new FlightAgencyEntities1();
        public bool LoginStaus;

        // GET: Portal
        public ActionResult Index(int TicketID, string PassFirstName, string PassLastName, string PassSex)
        {
            List<FlightClass> FlightClassList = new List<FlightClass>();
            List<InternalCity> InternalCityList = new List<InternalCity>();
            List<ExternalCity> ExternalCityList = new List<ExternalCity>();
            List<Passenger> PassengerList = new List<Passenger>();

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
            foreach (var Passenger in db.Passengers.Where(a => a.P_TicketID == TicketID))
            {
                PassengerList.Add(Passenger);
            }
            ViewBag.FlightClass = FlightClassList;
            ViewBag.InternalCity = InternalCityList;
            ViewBag.ExternalCity = ExternalCityList;
            ViewBag.Passengers = PassengerList;
            ViewBag.LoginStatus = LoginStaus;
            ViewBag.PassFisrtName = PassFirstName;
            ViewBag.PassLastName = PassLastName;
            if (PassSex == "مرد")
            {
                ViewBag.PassSex = "آقای";
            }
            else
            {
                ViewBag.PassSex = "خانم";
            }
            LoginStaus = false;
            return View();
        }

        // GET: Portal/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Passenger passenger = db.Passengers.Find(id);
            if (passenger == null)
            {
                return HttpNotFound();
            }
            return View(passenger);
        }

        // GET: Portal/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Portal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "P_ID,P_FirstName,P_LastName,P_FatherName,P_IdentityCode,P_Sexuality,P_Age,P_PhoneNumber,P_Adderess,P_Password,P_Email")] Passenger passenger)
        {
            if (ModelState.IsValid)
            {
                db.Passengers.Add(passenger);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(passenger);
        }

        // GET: Portal/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Passenger passenger = db.Passengers.Find(id);
            if (passenger == null)
            {
                return HttpNotFound();
            }
            return View(passenger);
        }

        // POST: Portal/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "P_ID,P_FirstName,P_LastName,P_FatherName,P_IdentityCode,P_Sexuality,P_Age,P_PhoneNumber,P_Adderess,P_Password,P_Email")] Passenger passenger)
        {
            if (ModelState.IsValid)
            {
                db.Entry(passenger).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(passenger);
        }

        // GET: Portal/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Passenger passenger = db.Passengers.Find(id);
            if (passenger == null)
            {
                return HttpNotFound();
            }
            return View(passenger);
        }

        // POST: Portal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Passenger passenger = db.Passengers.Find(id);
            db.Passengers.Remove(passenger);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
