using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminLTE9.Models;
using System.Net.Mail;
using System.Threading.Tasks;

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
                        return RedirectToAction("Index", "Portal", new { TicketID = v2.P_TicketID, PassFirstName = v2.P_FirstName, PassLastName = v2.P_LastName, PassSex = v2.P_Sexuality });
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

            var checkFlightExist = db.Flights.Where(a => a.F_Origin == Source && a.F_Destination == Destination /*&& a.F_Date == Date*/ && (Class != "همه" ? a.F_Class == Class : true) && a.F_Capacity > AdultNumber + KidNumber + LarvaNumber).FirstOrDefault();
            //var checkFlightExist = db.Flights.Where(a => a.F_Class == Class).FirstOrDefault();


            //var checkFlightExist = db.Flights.Where(a => a.F_Origin == Source && a.F_Destination == Destination && a.F_Date == Date && a.F_Capacity > AdultNumber + KidNumber + LarvaNumber).FirstOrDefault();

            if (checkFlightExist != null)
            {
                foreach (var flight in db.Flights)
                {
                    if (flight.F_Origin == Source
                        && flight.F_Destination == Destination
                        && /*flight.F_Date == Date &&*/ (Class != "همه" ? flight.F_Class == Class : true) && flight.F_Capacity > AdultNumber + KidNumber + LarvaNumber)
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
        public ActionResult AddPassengerToFlightAsync(PassAirFlight passAirFlight, string EmailAdult1)
        {
            //if (ModelState.IsValid)
            //{
            //    var body = "<p>{0} عزیز</p><p>بلیط شما در سامانه ثبت شد</p><p>برای پرینت بلیط پرواز با اطلاعات ارسال شده وارد پرتال وبسایت شده و پرینت بلیط خود را تهیه کنید</p><p>نام کاربری: {1}</p><p>رمز عبور: {2}</p>";
            //    var message = new MailMessage();
            //    message.To.Add(new MailAddress(EmailAdult1));
            //    message.From = new MailAddress("ahmadreza.anisi1996@outlook.com");
            //    message.Subject = "تاییدیه ثبت بلیط";
            //    message.Body = string.Format(body, passAirFlight.Passenger.P_FirstName + passAirFlight.Passenger.P_LastName, passAirFlight.Passenger.P_IdentityCode, 123456);
            //    message.IsBodyHtml = true;

            //    using (var smtp = new SmtpClient())
            //    {
            //        var credential = new NetworkCredential
            //        {
            //            UserName = "ahmadreza.anisi1996@outlook.com",
            //            Password = "`@Ahmadreza23101374"
            //        };
            //        smtp.Credentials = credential;
            //        smtp.Host = "smtp-mail.gmail.com";
            //        smtp.Port = 587;
            //        smtp.EnableSsl = true;
            //        await smtp.SendMailAsync(message);
            //        return RedirectToAction("Sent");
            //    }
            //}
            return RedirectToAction("SendEmail");
        }

        public ActionResult SendEmail()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendEmail(Passenger passenger)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("ahmadreza.anisih@gmail.com"));  // replace with valid value 
                message.From = new MailAddress("ahmadreza.anisi1996@outlook.com");  // replace with valid value
                message.Subject = "First Subject";
                message.Body = body; //string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "ahmadreza.anisi1996@outlook.com",  // replace with valid value
                        Password = "`@Ahmadreza23101374"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp-mail.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Sent");
                }
            }
            return View();
        }


        public ActionResult SaveAndSentEmail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveAndSendEmail(PassAirFlight passAirFlight, string SexualityAdult1, string EmailAdult1)
        {
            string z = passAirFlight.Flight.F_Origin;
            string y = passAirFlight.Passenger.P_FirstName;
            if(!ToAdmin(passAirFlight, SexualityAdult1, EmailAdult1))
            {
                ViewBag.Error = "";
                return View();
            }

            return RedirectToAction("Success", "Home");
        }

        public bool ToAdmin(PassAirFlight passAirFlight, string SexualityAdult1, string EmailAdult1)
        {
            bool Status = false;
            string body = "<p>{0} عزیز</p><p>بلیط شما در سامانه ثبت شد</p><p>برای پرینت بلیط پرواز با اطلاعات ارسال شده وارد پرتال وبسایت شده و پرینت بلیط خود را تهیه کنید</p><p>نام کاربری: {1}</p><p>رمز عبور: {2}</p>";
            var m = new MailMessage()
            {

                Subject = "تاییدیه ثبت بلیط",
                Body = string.Format(body, passAirFlight.Passenger.P_FirstName + passAirFlight.Passenger.P_LastName, passAirFlight.Passenger.P_IdentityCode, 123456),
                IsBodyHtml = true
            };
            //string to = new MailAddress("ahmadreza.anisih@gmail.com");
            m.To.Add(EmailAdult1);
            m.From = new MailAddress("ahmadreza.anisi1996@outlook.com");
            m.Sender = new MailAddress(EmailAdult1);


            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp-mail.outlook.com",
                Port = 587,
                Credentials = new NetworkCredential("ahmadreza.anisi1996@outlook.com", "`@Ahmadreza23101374"),
                EnableSsl = true
            };

            try
            {
                smtp.Send(m);
                Status = true;
            }
            catch (Exception e)
            {
                
            }
            return Status;
        }


















        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(Passenger model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("ahmadreza.anisih@gmail.com"));  // replace with valid value 
                message.From = new MailAddress("ahmadreza.anisi1996@outlook.com");  // replace with valid value
                message.Subject = "First Subject";
                message.Body = body; //string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "ahmadreza.anisi1996@outlook.com",  // replace with valid value
                        Password = "`@Ahmadreza23101374"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp-mail.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    //await smtp.SendMailAsync(message);
                    Task.Run(async () => { await smtp.SendMailAsync(message); }).Wait();
                    return RedirectToAction("Sent");
                }
            }
            return View(model);
        }

        public ActionResult Sent()
        {
            return View();
        }










    }
}
