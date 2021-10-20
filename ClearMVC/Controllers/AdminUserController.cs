using ClearMVC.Models;
using ClearMVC.ModelsVM;
using kindergarden.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ClearMVC.Controllers
{
    [LoginCheck]
    public class AdminUserController : Controller
    {
        private readonly ClearModelContext db = new ClearModelContext();


        [HttpPost]
        public string CheckUser(string email)
        {
            string result = string.Empty;
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            var model = db.Users.Where(p => p.FirmaId == firmaId && p.Email == email).FirstOrDefault();
            if (model!=null)
            {
                result = "Existing";
            }
            else
            {
                result = "Success";
            }


            return result;
        }

        public ActionResult DeleteJobs(DateTime? start, DateTime? end, int? CustomerId, int? ProjectId)
        {
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            ViewBag.Customer = new SelectList(db.Customers.Where(p => p.FirmaId == firmaId).ToList(), "CustomersId", "Name");
            List<ProjectDetails> model = GetProjectDetails(CustomerId, ProjectId, start, end);
            if (start != null && end != null)
            {
                ViewBag.Start = start;
                ViewBag.End = end;
                ViewBag.ProjectId = ProjectId;
            }
            return View(model);
        }
        
        [HttpPost]
        public ActionResult DeleteJobsDb(DateTime start, DateTime end, int projectId)
        {
            var model = db.ProjectDetails.Where(p =>p.ProjectId == projectId && p.StartDate >= start && p.FinishDate <= end).ToList();
            db.ProjectDetails.RemoveRange(model);
            db.SaveChanges();
            return RedirectToAction("ManagerPage","Home");
        
        }

        private List<ProjectDetails> GetProjectDetails(int? customerId, int? projectId, DateTime? start, DateTime? end)
        {
            List<ProjectDetails> data = new List<ProjectDetails>();
            if (start != null && end != null)
            {
                data = db.ProjectDetails.Include("Projects").Where(p => p.ProjectId == projectId && p.StartDate >= start && p.FinishDate <= end).ToList();
            }
            else if (start != null && end == null)
            {
                data = db.ProjectDetails.Include("Projects").Where(p => p.ProjectId == projectId && p.StartDate >= start).ToList();
            }
            else if (start == null && end != null)
            {
                data = db.ProjectDetails.Include("Projects").Where(p => p.ProjectId == projectId && p.FinishDate <= end).ToList();
            }
            else
            {
                data = db.ProjectDetails.Include("Projects").Where(p => p.ProjectId == projectId).ToList();
            }
            return data;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteJobs(DateTime? start, DateTime? end, int? CustomerId, int ProjectId)
        {
            return null;
        }


        public JsonResult ProjectsByCustomerId(int customerId)
        {
            var projects = db.Projects.Where(s => s.CustomerId == customerId).Select(s => new
            {
                id = s.ProjectsId,
                projectName = s.Name
            }).ToList();

            return Json(projects, JsonRequestBehavior.AllowGet);
        }




        public ActionResult MyJobs()
        {
            IEnumerable<ProjectDetails> model = new List<ProjectDetails>();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyJobs(DateTime? startDate, DateTime? finishDate, string status)
        {
            IEnumerable<ProjectDetails> model = new List<ProjectDetails>();
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            switch (status)
            {
                case "all":
                    if (startDate == null && finishDate == null)
                    {
                        model = db.ProjectDetails.Include("Projects").Where(p => p.Projects.FirmaId == firmaId).ToList();
                    }
                    else if (startDate != null && finishDate == null)
                    {
                        model = db.ProjectDetails.Include("Projects").Where(p => p.Projects.FirmaId == firmaId && p.StartDate >= startDate).ToList();
                    }
                    else if (startDate == null && finishDate != null)
                    {
                        model = db.ProjectDetails.Include("Projects").Where(p => p.Projects.FirmaId == firmaId && p.FinishDate <= finishDate).ToList();
                    }
                    else
                    {
                        model = db.ProjectDetails.Include("Projects").Where(p => p.Projects.FirmaId == firmaId && p.FinishDate <= finishDate && p.StartDate >= startDate).ToList();
                    }
                    break;
                case "refuse":
                    if (startDate == null && finishDate == null)
                    {
                        model = db.ProjectDetails.Include("Projects").Where(p => p.Projects.FirmaId == firmaId && p.IsRefuse == true).ToList();
                    }
                    else if (startDate != null && finishDate == null)
                    {
                        model = db.ProjectDetails.Include("Projects").Where(p => p.Projects.FirmaId == firmaId && p.StartDate >= startDate && p.IsRefuse == true).ToList();
                    }
                    else if (startDate == null && finishDate != null)
                    {
                        model = db.ProjectDetails.Include("Projects").Where(p => p.Projects.FirmaId == firmaId && p.FinishDate <= finishDate && p.IsRefuse == true).ToList();
                    }
                    else
                    {
                        model = db.ProjectDetails.Include("Projects").Where(p => p.Projects.FirmaId == firmaId && p.FinishDate <= finishDate && p.StartDate >= startDate && p.IsRefuse == true).ToList();
                    }
                    break;
                case "nonrefuse":
                    if (startDate == null && finishDate == null)
                    {
                        model = db.ProjectDetails.Include("Projects").Where(p => p.Projects.FirmaId == firmaId && p.IsRefuse == false).ToList();
                    }
                    else if (startDate != null && finishDate == null)
                    {
                        model = db.ProjectDetails.Include("Projects").Where(p => p.Projects.FirmaId == firmaId && p.StartDate >= startDate && p.IsRefuse == false).ToList();
                    }
                    else if (startDate == null && finishDate != null)
                    {
                        model = db.ProjectDetails.Include("Projects").Where(p => p.Projects.FirmaId == firmaId && p.FinishDate <= finishDate && p.IsRefuse == false).ToList();
                    }
                    else
                    {
                        model = db.ProjectDetails.Include("Projects").Where(p => p.Projects.FirmaId == firmaId && p.FinishDate <= finishDate && p.StartDate >= startDate && p.IsRefuse == false).ToList();
                    }
                    break;
                default:
                    break;
            }
            return View(model.OrderBy(p=>p.StartDate));
        }

        #region UserOperations
        public ActionResult ListUsers()
        {
            int userId = Convert.ToInt32(Session["userId"]);
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            var model = db.Users.Include("UserTypes").Where(p => p.UserTypes.Type != "Master" && p.FirmaId == firmaId && p.UsersId != userId && p.IsActive == true).ToList();
            return View(model);
        }
        public ActionResult CreateUser()
        {
            ViewBag.UserType = new SelectList(db.UserTypes.Where(p => p.Type != "Master").ToList(), "UserTypesId", "Type");
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateUser(UserVM model)
        {
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            if (ModelState.IsValid)
            {
                model.Users.CreateDate = DateTime.Now;
                model.Users.IsActive = true;
                model.Users.FirmaId = firmaId;
                model.Users.Password = "1234";
                db.Users.Add(model.Users);
                db.SaveChanges();
                int UserId = model.Users.UsersId;
                CreateUserDetails(model.UserDetails, UserId);
                return RedirectToAction("ListUsers");
            }
            ViewBag.UserType = new SelectList(db.UserTypes.Where(p => p.Type != "Master").ToList(), "UserTypesId", "Type");
            return View(model);
        }

        private void CreateUserDetails(UserDetails userDetails, int userId)
        {
            userDetails.UserId = userId;
            db.UserDetails.Add(userDetails);
            db.SaveChanges();
        }

        public ActionResult ChangeStatusUsers(int Id)
        {
            var model = db.Users.Find(Id);
            if (model.IsActive == true)
            {
                model.IsActive = false;
            }
            else
            {
                model.IsActive = true;
            }
            db.SaveChanges();
            return RedirectToAction("ListUsers");
        }
        public ActionResult ChangePassword(int Id)
        {
            var model = db.Users.Find(Id);
            model.Password = "1234";
            db.SaveChanges();
            return RedirectToAction("ListUsers");
        }
        #endregion

        #region ListProjectTypesOperations
        public ActionResult ListProjectTypes()
        {
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            var model = db.ProjectDetailsTypes.Where(p => p.FirmaId == firmaId).ToList();
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateProjectDetailType(ProjectDetailsTypes model)
        {
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            model.CreatedDate = DateTime.Now;
            model.IsActive = true;
            model.FirmaId = firmaId;
            db.ProjectDetailsTypes.Add(model);
            db.SaveChanges();
            return RedirectToAction("ListProjectTypes");
        }
        public ActionResult DeleteProjectDetailType(int Id)
        {
            var model = db.ProjectDetailsTypes.Find(Id);
            db.ProjectDetailsTypes.Remove(model);
            db.SaveChanges();
            return RedirectToAction("ListProjectTypes");
        }
        #endregion

        #region ProjectOperations

        public ActionResult FollowRefuseByManager(int projectDetailId)
        {
            var model = db.Refuses.Include("ProjectDetails").Where(p => p.ProjectDetailsId == projectDetailId).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FollowRefuseByManager(Refuse refuse)
        {
            if (ModelState.IsValid)
            {
                refuse.CreatedBy = Convert.ToString(Session["role"] + " / " + Session["userName"]);
                db.Refuses.Add(refuse);
                db.SaveChanges();
            }
            return RedirectToAction("ProjectDetail", new { Id = refuse.ProjectId });
        }

        public ActionResult ListProjects()
        {
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            var model = db.Projects.Include("Customers").Where(p => p.FirmaId == firmaId).ToList();
            return View(model);
        }

        public ActionResult CreateProject()
        {
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            ViewBag.FirmaId = firmaId;
            ViewBag.Customers = new SelectList(db.Customers.Where(p => p.FirmaId == firmaId).ToList(), "CustomersId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProject(ProjectsVM model)
        {
            if (ModelState.IsValid)
            {
                model.Projects.CreateDate = DateTime.Now;
                model.Projects.IsActive = true;
                db.Projects.Add(model.Projects);
                db.SaveChanges();
                int projectId = model.Projects.ProjectsId;
                model.Contacts.ProjectId = projectId;
                CrateProjectContact(model.Contacts);
                return RedirectToAction("ListProjects");
            }
            //ViewBag.Customers = new SelectList(db.Customers.Where(p => p.FirmaId == model.FirmaId), "CustomersId", "Name").ToList();
            return View();
        }

        private void CrateProjectContact(Contacts contacts)
        {
            contacts.CreateDate = DateTime.Now;
            contacts.CreatedBy = Convert.ToString(Session["userName"]);
            contacts.IsActive = true;
            db.Contacts.Add(contacts);
            db.SaveChanges();
        }

        public ActionResult ProjectDetail(int Id)
        {
            var project = db.Projects.Find(Id);
            ProjectDetailVM projectDetailVM = new ProjectDetailVM
            {
                Contact = project.Contacts.First(),
                Customer = project.Customers,
                Project = project
            };
            return View(projectDetailVM);
        }

        public ActionResult DetailProjectDetailItems(int Id)
        {
            var model = db.ProjectDetails.Include("Refuses").Where(p=>p.ProjectDetailsId == Id).FirstOrDefault();
            return View(model);
        }

        [ChildActionOnly]
        public PartialViewResult ProjectDetailItems(int Id)
        {
            var model = db.ProjectDetails.Include("Users").Include("ProjectDetailsTypes").Where(p => p.ProjectId == Id).OrderBy(p=>p.StartDate).ToList();
            return PartialView("_ProjectDetailItems", model);
        }

        public ActionResult CreateProjectDetailItems(int projectId)
        {
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            ViewBag.ProjectId = projectId;
            ViewBag.Person = new SelectList(db.Users.Include("UserTypes").Where(p => p.UserTypes.Type == "Personal" && p.IsActive == true && p.FirmaId == firmaId).ToList(), "UsersId", "Email");
            ViewBag.Type = new SelectList(db.ProjectDetailsTypes.Where(p => p.IsActive == true && p.FirmaId == firmaId).ToList(), "ProjectDetailsTypesId", "Type");
            return PartialView("_CreateProjectDetailItems");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProjectDetailItems(ProjectDetails projectDetails)
        {
            DateTime startDate = projectDetails.StartDate.Value;
            DateTime finishDate = projectDetails.FinishDate.Value;
            if (ModelState.IsValid)
            {
                if ((projectDetails.StartDate.Value.Day == projectDetails.FinishDate.Value.Day) && (projectDetails.StartDate.Value.Month == projectDetails.FinishDate.Value.Month) &&
                    (projectDetails.StartDate.Value.Year == projectDetails.FinishDate.Value.Year))
                {
                    projectDetails.CreatedBy = Convert.ToString(Session["userName"]);
                    projectDetails.CreatedDate = DateTime.Now;
                    projectDetails.IsActive = true;
                    projectDetails.IsCompleted = false;
                    projectDetails.CompleteDate = new DateTime(1000, 01, 01, 01, 01, 01);
                    db.ProjectDetails.Add(projectDetails);
                    db.SaveChanges();
                }
                else
                {
                    List<ProjectDetails> pdetailList = new List<ProjectDetails>();
                    while (startDate <= finishDate)
                    {
                        ProjectDetails repeatProjectDetails = new ProjectDetails();
                        repeatProjectDetails.CreatedBy = Convert.ToString(Session["userName"]);
                        repeatProjectDetails.CreatedDate = DateTime.Now;
                        repeatProjectDetails.IsActive = true;
                        repeatProjectDetails.IsRefuse = false;
                        repeatProjectDetails.IsCompleted = false;
                        repeatProjectDetails.CompleteDate = new DateTime(1000, 01, 01, 01, 01, 01);
                        repeatProjectDetails.Duration = projectDetails.Duration;
                        repeatProjectDetails.StartDate = startDate;
                        //startdatetime a finisdatein saati gelmesi lazim
                        repeatProjectDetails.FinishDate = startDate.Date.Add(new TimeSpan(finishDate.Hour, finishDate.Minute, finishDate.Second));
                        repeatProjectDetails.Square = projectDetails.Square;
                        repeatProjectDetails.PersonId = projectDetails.PersonId;
                        repeatProjectDetails.ProjectDetailTypeId = projectDetails.ProjectDetailTypeId;
                        repeatProjectDetails.ProjectId = projectDetails.ProjectId;
                        pdetailList.Add(repeatProjectDetails);
                        startDate = startDate.AddDays(1);
                    }
                    db.ProjectDetails.AddRange(pdetailList);
                    db.SaveChanges();
                }
                return RedirectToAction("ProjectDetail", new { Id = projectDetails.ProjectId });
            }
            return RedirectToAction("ListProjects");
        }

        public ActionResult DeleteProjectDetail(int Id)
        {
            var model = db.ProjectDetails.Find(Id);
            int projectId = (int)model.ProjectId;
            db.ProjectDetails.Remove(model);
            db.SaveChanges();
            return RedirectToAction("ProjectDetail", new { Id = projectId });
        }

        #endregion

        #region CustomerOperations
        public ActionResult ListCustomers()
        {
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            var model = db.Customers.Where(p => p.FirmaId == firmaId).ToList();
            return View(model);
        }
        public ActionResult ChangeStatusCustomer(int Id)
        {
            var model = db.Customers.Find(Id);
            if (model.IsActive == true)
            {
                model.IsActive = false;
            }
            else
            {
                model.IsActive = true;
            }
            db.SaveChanges();
            return RedirectToAction("ListCustomers");
        }
        public ActionResult CreateCustomer()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateCustomer(Customers model)
        {
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.IsActive = true;
                model.FirmaId = firmaId;
                model.Password = "1234";
                db.Customers.Add(model);
                db.SaveChanges();
                return RedirectToAction("ListCustomers");
            }
            return View(model);
        }
        #endregion

        #region NewsOperations

        public ActionResult ListNews()
        {
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            var model = db.News.Where(p => p.FirmaId == firmaId).ToList();
            return View(model);
        }

        public ActionResult CreateNews()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNews(News news)
        {
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            if (ModelState.IsValid)
            {
                news.CreateDate = DateTime.Now;
                news.IsActive = true;
                news.FirmaId = firmaId;

                db.News.Add(news);
                db.SaveChanges();
                return RedirectToAction("ListNews");
            }
            return Content("Error");
        }

        public ActionResult DeleteNews(int Id)
        {
            var model = db.News.Find(Id);
            db.News.Remove(model);
            db.SaveChanges();
            return RedirectToAction("ListNews");
        }
        #endregion

        #region ActivitiesOperations

        public ActionResult ListActivities()
        {
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            var model = db.Activities.Where(p => p.FirmaId == firmaId).ToList();
            return View(model);
        }

        public ActionResult CreateActivities()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateActivities(Activities activities)
        {
            int firmaId = Convert.ToInt32(Session["firmaId"]);
            if (ModelState.IsValid)
            {
                activities.CreatedDate = DateTime.Now;
                activities.IsActive = true;
                activities.FirmaId = firmaId;
                db.Activities.Add(activities);
                db.SaveChanges();
                return RedirectToAction("ListActivities");
            }
            return Content("Error");
        }

        public ActionResult DeleteActivities(int Id)
        {
            var model = db.Activities.Find(Id);
            db.Activities.Remove(model);
            db.SaveChanges();
            return RedirectToAction("ListActivities");
        }
        #endregion

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