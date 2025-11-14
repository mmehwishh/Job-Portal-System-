using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Job_Portal_Managment_System;

//Dashboard()	Overview of jobs, applications, notifications
//CreateProfile()	Add or edit employer/company details
//MyJobs()	View all posted jobs
//Applicants(jobId)	View who applied for a specific job


namespace Job_Portal_Managment_System.Controllers
{
    [Authorize]

    public class EmployeerController : Controller
    {
        private JobDBEntities db = new JobDBEntities();

        // GET: /Employeer/
        public ActionResult Index()
        {
            var employers = db.EMPLOYERS.Include(e => e.USER);
            return View(employers.ToList());
        }

        // GET: /Employeer/Dashboard

        public ActionResult Dashboard()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            var emp = db.EMPLOYERS.FirstOrDefault(e => e.employer_id == UserId);

            if (emp == null)
                return RedirectToAction("Login" , "Account"); // or handle null appropriately

            var totalJobs = db.JOBS.Count(j => j.employer_id == UserId);

            int employerId = emp.employer_id;  // <-- store in a local variable

            var appRcv = from a in db.APPLICATIONS
                         join j in db.JOBS on a.job_id equals j.job_id
                         where j.employer_id == employerId   // <-- use local variable
                         select a;

            int appRcvCount = appRcv.Count();

            int unreadNotf = db.NOTIFICATIONS.Count(n => n.user_id == employerId && n.read_status == "UNREAD");

            ViewBag.TotalJobs = totalJobs;
            ViewBag.TotalApplication = appRcvCount;
            ViewBag.UnreadNotifications = unreadNotf;
            ViewBag.CompanyName = emp.company_name;

            return View();
        }


        public ActionResult CreateProfile()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProfile(EMPLOYER employer)
        {
            if(ModelState.IsValid)
            {
                employer.employer_id = Convert.ToInt32(Session["UserId"]);
                db.EMPLOYERS.Add(employer);
                db.SaveChanges();

                return RedirectToAction("Dashboard");
            }
            return View(employer);
        }

        public ActionResult EditProfile()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            var emp = db.EMPLOYERS.FirstOrDefault(e => e.employer_id == userId);
            if (emp == null )
            {
                return HttpNotFound();
            }
            return View(emp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(EMPLOYER employer)
        {
            if(ModelState.IsValid)
            {
                db.Entry(employer).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "Profile updated succesfully";
                return RedirectToAction("Dashboard");
            }
            return View(employer);
        }

        public ActionResult PostJob()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostJob(JOB job)
        {
            if (ModelState.IsValid)
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                job.employer_id = userId;
                job.posted_date = DateTime.Now;

                db.JOBS.Add(job);
                db.SaveChanges();
                TempData["Success"] = "Job posted successfully!";
                return RedirectToAction("Dashboard");
            }
            return View(job);
        }

        public ActionResult MyJobs()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            var jobs = db.JOBS.Where(j => j.employer_id == userId).ToList();
            return View(jobs);
        }

        public ActionResult EditJob(int id)
        {
            var job = db.JOBS.Find(id);
            if(job == null )
            {
                return HttpNotFound();
            }
            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditJob(JOB job)
        {
            if(ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyJobs");
            }
            return View(job);
        }

        public ActionResult DeleteJob(int id)
        {
            var job = db.JOBS.Find(id);
            if(job == null )
            {
                return HttpNotFound();
            }
            db.JOBS.Remove(job);
            db.SaveChanges();
            return RedirectToAction("MyJobs");
        }

        public class ApplicantViewModel
        {
            public int seekerId { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public DateTime AppliedDate { get; set; }
            public string resume { get; set; }
        }


        public ActionResult Applicants(int jobId)
        {
            var applicants = from a in db.APPLICATIONS
                             join s in db.JOB_SEEKER_PROFILE on a.seeker_id equals s.seeker_id
                             join u in db.USERS on s.seeker_id equals u.UserId
                             where a.job_id == jobId
                             select new ApplicantViewModel
                             {
                                 seekerId = s.seeker_id,
                                 FullName = u.full_name,
                                 Email = u.email,
                                 AppliedDate = (DateTime)a.applied_date,
                                 resume = s.resume_file

                             };
            var jobT = db.JOBS.FirstOrDefault(j => j.job_id == jobId);
            ViewBag.JobTitle = jobT.job_title;
            return View(applicants.ToList());
        }

        public ActionResult ViewApplicant(int seekerId)
        {
            var seeker = db.JOB_SEEKER_PROFILE.FirstOrDefault(j => j.seeker_id == seekerId);
            if(seeker == null )
            {
                return HttpNotFound();
            }
            return View(seeker);
        }

        public ActionResult Notifications()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            var notifications = db.NOTIFICATIONS
                                .Where(n => n.user_id == userId)
                                .OrderByDescending(n=> n.created_at)
                                .ToList();
            foreach(var n in notifications)
            {
                n.read_status = "READ";
            }
            db.SaveChanges();
            return View(notifications);

        }



        // GET: /Employeer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLOYER employer = db.EMPLOYERS.Find(id);
            if (employer == null)
            {
                return HttpNotFound();
            }
            return View(employer);
        }

        // GET: /Employeer/Create
        public ActionResult Create()
        {
            ViewBag.employer_id = new SelectList(db.USERS, "UserId", "full_name");
            return View();
        }

        // POST: /Employeer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="employer_id,company_name,industry,company_description,website,location")] EMPLOYER employer)
        {
            if (ModelState.IsValid)
            {
                db.EMPLOYERS.Add(employer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.employer_id = new SelectList(db.USERS, "UserId", "full_name", employer.employer_id);
            return View(employer);
        }

        // GET: /Employeer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLOYER employer = db.EMPLOYERS.Find(id);
            if (employer == null)
            {
                return HttpNotFound();
            }
            ViewBag.employer_id = new SelectList(db.USERS, "UserId", "full_name", employer.employer_id);
            return View(employer);
        }

        // POST: /Employeer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="employer_id,company_name,industry,company_description,website,location")] EMPLOYER employer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.employer_id = new SelectList(db.USERS, "UserId", "full_name", employer.employer_id);
            return View(employer);
        }

        // GET: /Employeer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLOYER employer = db.EMPLOYERS.Find(id);
            if (employer == null)
            {
                return HttpNotFound();
            }
            return View(employer);
        }

        // POST: /Employeer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EMPLOYER employer = db.EMPLOYERS.Find(id);
            db.EMPLOYERS.Remove(employer);
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
