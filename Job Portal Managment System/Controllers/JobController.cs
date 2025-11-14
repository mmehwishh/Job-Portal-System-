using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Job_Portal_Managment_System;

namespace Job_Portal_Managment_System.Controllers
{
    public class JobController : Controller
    {
        private JobDBEntities db = new JobDBEntities();

        // GET: /Job/
        public ActionResult Index()
        {
            var jobs = db.JOBS.Include(j => j.EMPLOYER);
            return View(jobs.ToList());
        }

        // GET: /Job/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JOB job = db.JOBS.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: /Job/Create
        public ActionResult Create()
        {
            ViewBag.employer_id = new SelectList(db.EMPLOYERS, "employer_id", "company_name");
            return View();
        }

        // POST: /Job/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="job_id,employer_id,job_title,job_description,requirements,location,salary_range,job_type,posted_date,expiry_date,status")] JOB job)
        {
            if (ModelState.IsValid)
            {
                db.JOBS.Add(job);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.employer_id = new SelectList(db.EMPLOYERS, "employer_id", "company_name", job.employer_id);
            return View(job);
        }

        // GET: /Job/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JOB job = db.JOBS.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            ViewBag.employer_id = new SelectList(db.EMPLOYERS, "employer_id", "company_name", job.employer_id);
            return View(job);
        }

        // POST: /Job/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="job_id,employer_id,job_title,job_description,requirements,location,salary_range,job_type,posted_date,expiry_date,status")] JOB job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.employer_id = new SelectList(db.EMPLOYERS, "employer_id", "company_name", job.employer_id);
            return View(job);
        }

        // GET: /Job/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JOB job = db.JOBS.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: /Job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JOB job = db.JOBS.Find(id);
            db.JOBS.Remove(job);
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
