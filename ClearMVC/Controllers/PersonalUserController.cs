using ClearMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Mvc;

namespace ClearMVC.Controllers
{
    public class PersonalUserController : Controller
    {
        private readonly ClearModelContext db = new ClearModelContext();
        public ActionResult FinishProcessByPersonal(int ProjectDetailId)
        {
            var model = db.ProjectDetails.Find(ProjectDetailId);
            model.IsCompleted = true;
            model.CompleteDate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("PersonalPage", "Home");
        }

        public ActionResult NewsForPersonal()
        {
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            var model = db.News.Where(p => p.FirmaId == firmaId && p.ForCustomer == false && p.IsActive == true).ToList();
            return View(model);
        }

        public ActionResult ActivitiesForPersonal()
        {
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            var model = db.Activities.Where(p => p.FirmaId == firmaId && p.ForCustomer == false && p.IsActive == true).ToList();
            return View(model);
        }

        public ActionResult MyJobs()
        {
            int personId = Convert.ToInt32(Session["userId"]);
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            var model = db.ProjectDetails.Include("Projects").Where(p => p.PersonId == personId && p.StartDate.Value.Month == month && p.StartDate.Value.Year == year).OrderBy(p=>p.StartDate).ToList();
            return View(model);
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