using ClearMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClearMVC.Controllers
{
    public class MasterUserController : Controller
    {
        private ClearModelContext db = new ClearModelContext();

        public ActionResult PendingApprove()
        {
            var model = db.Firmas.Where(p => p.IsActive == false).ToList();
            return View(model);
        }

        public ActionResult ApproveCompany(int firmaId)
        {
            var firma = db.Firmas.Find(firmaId);
            ApproveManager(firmaId);
            firma.IsActive = true;
            db.SaveChanges();
            return RedirectToAction("MasterPage", "Home");
        }

        private void ApproveManager(int firmaId)
        {
            var manager = db.Users.Where(p => p.FirmaId == firmaId).FirstOrDefault();
            manager.IsActive = true;
            db.SaveChanges();
        }

        public ActionResult DenyCompany(int firmaId)
        {
            var firma = db.Firmas.Find(firmaId);
            DenyManager(firmaId);
            db.Firmas.Remove(firma);
            db.SaveChanges();
            return RedirectToAction("MasterPage", "Home");
        }

        private void DenyManager(int firmaId)
        {
            var manager = db.Users.Where(p => p.FirmaId == firmaId).FirstOrDefault();
            db.Users.Remove(manager);
            db.SaveChanges();
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