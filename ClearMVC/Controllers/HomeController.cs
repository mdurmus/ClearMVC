using ClearMVC.Models;
using ClearMVC.ModelsVM;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClearMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ClearModelContext db = new ClearModelContext();

        public ActionResult MasterPage()
        {
            return View();
        }

        public ActionResult ManagerPage()
        {
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            ViewBag.ProjectCount = db.Projects.Where(p => p.FirmaId == firmaId).Count();
            ViewBag.EmployeeCount = db.Users.Include("UserTypes").Where(p => p.FirmaId == firmaId && p.UserTypes.Type == "Personal").Count();
            ViewBag.CustomerCount = db.Customers.Where(p => p.FirmaId == firmaId).Count();
            ViewBag.RefuseCount = GetRefusedProjectCount(firmaId);
            List<AdminDashboardRefuseVM> model = GetAdminDashboardRefuse();
            return View(model);
        }

        private List<AdminDashboardRefuseVM> GetAdminDashboardRefuse()
        {
            List<AdminDashboardRefuseVM> data = new List<AdminDashboardRefuseVM>();
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            var model = db.Projects.Include("ProjectDetails").Where(p => p.FirmaId == firmaId).ToList();
            foreach (var item in model)
            {
                foreach (var item2 in item.ProjectDetails)
                {
                    if (item2.IsRefuse == true && item2.IsCloseRefuseByCustomer == false)
                    {
                        AdminDashboardRefuseVM adrvm = new AdminDashboardRefuseVM();
                        adrvm.ProjectId = item.ProjectsId;
                        adrvm.Customer = item.Customers.Name;
                        adrvm.Employee = item2.Users.Name + " " + item2.Users.LastName;
                        adrvm.ProjectdetailId = item2.ProjectDetailsId;
                        adrvm.ProjectName = item.Name;
                        data.Add(adrvm);
                    }
                }
            }
            return data;
        }

        private int GetRefusedProjectCount(int firmaId)
        {
            int count = 0;
            var model = db.Projects.Include("ProjectDetails").Where(p => p.FirmaId == firmaId).ToList();
            foreach (var item in model)
            {
                foreach (var item2 in item.ProjectDetails)
                {
                    if (item2.IsRefuse == true)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public ActionResult PersonalPage()
        {
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            int userId = Convert.ToInt32(Session["userId"]);
            ViewBag.News = db.News.Where(p => p.FirmaId == firmaId && p.ForCustomer == false).Count();
            ViewBag.Activities = db.Activities.Where(p => p.FirmaId == firmaId && p.ForCustomer == false).Count();
            ViewBag.ProjectDetails = db.ProjectDetails.Where(p => p.PersonId == userId).Count();
            IEnumerable<DailyJobVM> model = GetDailyJobForPersonalId(userId);
            return View(model);
        }

        private IEnumerable<DailyJobVM> GetDailyJobForPersonalId(int userId)
        {
            List<DailyJobVM> taskList = new List<DailyJobVM>();
            var model = db.ProjectDetails.Include("Projects").Where(p => p.PersonId == userId && p.IsCompleted == false && (p.StartDate.Value.Day == DateTime.Now.Day && p.StartDate.Value.Month == DateTime.Now.Month && p.StartDate.Value.Year == DateTime.Now.Year)).ToList();
            foreach (var item in model)
            {
                DailyJobVM job = new DailyJobVM
                {
                    CustomerName = item.Projects.Name,
                    Location = item.Projects.Location,
                    StartTime = (DateTime)item.StartDate,
                    FinishTime = (DateTime)item.FinishDate,
                    ProjectDetailId = item.ProjectDetailsId,
                    Duration = (double)item.Duration,
                    ForRefuse = item.ForRefuse
                };
                taskList.Add(job);
            }
            return taskList;
        }

        public ActionResult KundenPage()
        {
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            int customerId = Convert.ToInt32(Session["userId"]);
            ViewBag.News = db.News.Where(p => p.FirmaId == firmaId && p.ForCustomer == true).Count();
            ViewBag.Activities = db.Activities.Where(p => p.FirmaId == firmaId && p.ForCustomer == true).Count();
            ViewBag.Projects = db.Projects.Where(p => p.CustomerId == customerId).Count();
            IEnumerable<DailyJobCustomerVM> model = GetDailyJobForCustomerId(customerId);
            return View(model);
        }

        private List<DailyJobCustomerVM> GetDailyJobForCustomerId(int customerId)
        {
            List<DailyJobCustomerVM> model = new List<DailyJobCustomerVM>();
            var poco = db.Projects.Where(p => p.CustomerId == customerId).ToList();
            foreach (var item in poco)
            {
                int count = GetProjectDetailCount(item.ProjectsId);
                DailyJobCustomerVM djcvm = new DailyJobCustomerVM
                {
                    Location = item.Location,
                    ProjectId = item.ProjectsId,
                    ProjectName = item.Name,
                    JobCount = count
                };
                model.Add(djcvm);
            }
            return model;
        }

        private int GetProjectDetailCount(int projectsId)
        {
            var projectDetailCount = db.ProjectDetails.Where(p => p.ProjectId == projectsId).Count();
            return projectDetailCount;
        }

        public ActionResult UserDetail(int Id, string type)
        {
            ViewBag.UserId = Id;
            ViewBag.Role = type;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserDetail(UserDetails model, string Role)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.IsActive = true;
                db.UserDetails.Add(model);
                db.SaveChanges();
                switch (Role)
                {
                    case "Master":
                        return RedirectToAction("MasterPage", "Home");
                    case "Manager":
                        return RedirectToAction("ManagerPage", "Home");
                    case "Personal":
                        return RedirectToAction("PersonalPage", "Home");
                    case "Kunden":
                        return RedirectToAction("KundenPage", "Home");
                }
            }
            return null;
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