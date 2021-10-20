using ClearMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClearMVC.Controllers
{
    public class KundenUserController : Controller
    {
        private readonly ClearModelContext db = new ClearModelContext();

        public ActionResult CloseRefuseCustomer(int ProjectDetailId)
        {
            var model = db.ProjectDetails.Find(ProjectDetailId);
            model.IsCloseRefuseByCustomer = true;
            db.SaveChanges();
            return RedirectToAction("ListProjectDetails",new { ProjectId=model.ProjectId });
        }


        public ActionResult FollowRefuseByCustomer(int projectDetailId)
        {
            var model = db.Refuses.Where(p => p.ProjectDetailsId == projectDetailId).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FollowRefuseByCustomer(Refuse refuse)
        {
            if (ModelState.IsValid)
            {
                refuse.CreatedBy = Convert.ToString(Session["role"]);
                db.Refuses.Add(refuse);
                db.SaveChanges();
            }
            return RedirectToAction("ListProjectDetails",new { ProjectId = refuse.ProjectId });
        }

        public ActionResult ListProjectDetails(int ProjectId)
        {
            var model = db.ProjectDetails.Include("ProjectDetailsTypes").Where(p => p.ProjectId == ProjectId).ToList();
            return View(model);
        }

        public ActionResult RefuseToProjectDetail(int projectDetailId)
        {
            var model = db.ProjectDetails.Find(projectDetailId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RefuseToProjectDetail(Refuse refse, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                refse.CreatedBy = Convert.ToString(Session["role"]+" / "+Session["userName"]);
                db.Refuses.Add(refse);
                db.SaveChanges();
                int refuseId = refse.RefuseId;
                UpdatePictureFiles(refuseId, photo);
                MarkProjectDetailAsRefuse(refse.ProjectDetailsId);
                return RedirectToAction("ListProjectDetails", new { ProjectId = refse.ProjectId });
            }
            return View(refse);
        }

        private void MarkProjectDetailAsRefuse(int projectDetailsId)
        {
            var model = db.ProjectDetails.Find(projectDetailsId);
            model.IsRefuse = true;
            db.SaveChanges();
        }

        private void UpdatePictureFiles(int refuseId, HttpPostedFileBase photo)
        {
            string root = "~/Reklamation/";
            string fileName = "Refuse_"+refuseId + ".jpg";
            photo.SaveAs(Server.MapPath(root+fileName));
            var model = db.Refuses.Find(refuseId);
            model.FileName = root + fileName;
            db.SaveChanges();
        }

        public ActionResult NewsForCustomer()
        {
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            var model = db.News.Where(p => p.FirmaId == firmaId && p.ForCustomer == true && p.IsActive == true).ToList();
            return View(model);
        }

        public ActionResult ActivitiesForCustomer()
        {
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            var model = db.Activities.Where(p => p.FirmaId == firmaId && p.ForCustomer == true && p.IsActive == true).ToList();
            return View(model);
        }

        public ActionResult MyJobs()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyJobs(DateTime? start, DateTime? end)
        {
            int customerId = Convert.ToInt32(Session["userId"]);
            List<ProjectDetails> model = new List<ProjectDetails>();
            int count = 0;
            if (start != null && end == null)
            {
                model = db.ProjectDetails.Include("Projects").Where(
                    k=>k.StartDate >= start && 
                    k.IsActive == true 
                    && k.Projects.CustomerId == customerId 
                    ).OrderBy(p => p.StartDate).ToList();
                count = model.Count;
            }
            else if (start != null && end != null)
            {
                model = db.ProjectDetails.Include("Projects").Where(
                    k => k.StartDate >= start &&
                    k.StartDate <= end &&
                    k.IsActive == true&&
                    k.Projects.CustomerId == customerId)
                    .OrderBy(p => p.StartDate).ToList();
                count = model.Count;
            }
            else
            {
                model = db.ProjectDetails.Include("Projects").Where( k=>k.IsActive == true && k.Projects.CustomerId == customerId).OrderBy(p => p.StartDate).ToList();
                count = model.Count;
            }
            ViewBag.Count = count;
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