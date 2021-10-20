using ClearMVC.Models;
using ClearMVC.ModelsVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ClearMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly ClearModelContext db = new ClearModelContext();

        public ActionResult Login(bool? result)
        {
            if (result == false)
            {
                ViewBag.Message = "Bitte überprüfen Sie Ihre Anmeldeinformationen.";
            }
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(string email, string pass)
        {
            var customer = db.Customers.Where(p => p.Email == email && p.Password == pass).FirstOrDefault();
            if (customer != null)
            {
                if (customer.Password == "1234")
                {
                    return RedirectToAction("SetPassword", "Login", new { rusername = customer.Email });
                }
                Session["userName"] = customer.Email;
                Session["role"] = "Kunden";
                Session["userId"] = customer.CustomersId;
                Session["firmaId"] = customer.FirmaId;
                return RedirectToAction("KundenPage", "Home");
            }
            var user = db.Users.Include("UserTypes").Include("UserDetails").Where(p => p.Email == email && p.Password == pass && p.IsActive == true).FirstOrDefault();
            if (user != null)
            {
                if (user.Password == "1234")
                {
                    return RedirectToAction("SetPassword", "Login", new { rusername = user.Email });
                }
                if (user.UserTypes.Type == "Master")
                {
                    Session["userName"] = user.Email;
                    Session["role"] = "Master";
                    Session["userId"] = user.UsersId;
                    Session["firmaId"] = user.FirmaId;
                    return RedirectToAction("MasterPage", "Home");
                }
                else if (user.UserTypes.Type == "Manager")
                {
                    Session["userName"] = user.Email;
                    Session["role"] = "Manager";
                    Session["userId"] = user.UsersId;
                    Session["firmaId"] = user.FirmaId;
                    return RedirectToAction("ManagerPage", "Home");
                }
                else if (user.UserTypes.Type == "Personal")
                {
                    Session["userName"] = user.Email;
                    Session["role"] = "Personal";
                    Session["userId"] = user.UsersId;
                    Session["firmaId"] = user.FirmaId;
                    return RedirectToAction("PersonalPage", "Home");
                }
            }
            return RedirectToAction("Login", new { result = false });
        }

        public ActionResult SetPassword(string rusername)
        {
            if (!string.IsNullOrEmpty(rusername))
            {
                ViewBag.rusername = rusername;
            }
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SetPassword(string email, string password)
        {
            var customer = db.Customers.Where(p => p.Email == email).FirstOrDefault();
            if (customer != null)
            {
                customer.Password = password;
            }
            else
            {
                var model = db.Users.Where(p => p.Email == email).FirstOrDefault();
                model.Password = password;
            }
            db.SaveChanges();
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Login");
        }

        public ActionResult CompanyRegister()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyRegister(FirmaRegisterVM register)
        {
            if (ModelState.IsValid)
            {
                int firmaId = SaveFirma(register.Firma);
                register.Users.FirmaId = firmaId;
                register.Users.UserTypesId = 2;
                register.Users.IsActive = false;
                register.Firma.IsActive = false;
                db.Users.Add(register.Users);
                db.SaveChanges();
                SendEmail(register);
                return RedirectToAction("Logout");
            }
            return View();
        }

        private void SendEmail(FirmaRegisterVM data)
        {
            MailMessage ePosta = new MailMessage();
            ePosta.From = new MailAddress("formrgmbh@gmail.com");
            ePosta.To.Add("bykingpin@gmail.com");
            ePosta.To.Add("heuseyin1986@gmail.com");
            ePosta.Subject = "Steril365 Firma Kayit";
            ePosta.Body = @"Firma Adi: " + data.Firma.Name + "<br>Firma telefonu: <a href='tel:" + data.Firma.Phone + " 'target='_blank'>" + data.Firma.Phone + "</a> <br><hr> admin adi: " + data.Users.Name + "<br> admin soyadi: " + data.Users.LastName;
            ePosta.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            NetworkCredential networkCredential = new NetworkCredential("formrgmbh@gmail.com", "Formr123?");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = networkCredential;
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            object userState = ePosta;
            try { smtp.Send(ePosta); }
            catch (Exception) { }
        }

        private int SaveFirma(Firma firma)
        {
            firma.IsActive = false;
            db.Firmas.Add(firma);
            db.SaveChanges();
            int firmaId = firma.Id;
            return firmaId;
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