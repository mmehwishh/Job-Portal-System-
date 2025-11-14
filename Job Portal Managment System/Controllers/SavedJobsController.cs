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
    public class SavedJobsController : Controller
    {
        private JobDBEntities db = new JobDBEntities();

        // GET: /SavedJobs/
        public ActionResult Index()
        {
            var saved_jobs = db.SAVED_JOBS.Include(s => s.JOB_SEEKER_PROFILE).Include(s => s.JOB);
            return View(saved_jobs.ToList());
        }

        // GET: /SavedJobs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SAVED_JOBS saved_jobs = db.SAVED_JOBS.Find(id);
            if (saved_jobs == null)
            {
                return HttpNotFound();
            }
            return View(saved_jobs);
        }

        // GET: /SavedJobs/Create
        public ActionResult Create()
        {
            ViewBag.seeker_id = new SelectList(db.JOB_SEEKER_PROFILE, "seeker_id", "gender");
            ViewBag.job_id = new SelectList(db.JOBS, "job_id", "job_title");
            return View();
        }

        // POST: /SavedJobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="seeker_id,job_id,saved_at")] SAVED_JOBS saved_jobs)
        {
            if (ModelState.IsValid)
            {
                db.SAVED_JOBS.Add(saved_jobs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.seeker_id = new SelectList(db.JOB_SEEKER_PROFILE, "seeker_id", "gender", saved_jobs.seeker_id);
            ViewBag.job_id = new SelectList(db.JOBS, "job_id", "job_title", saved_jobs.job_id);
            return View(saved_jobs);
        }

        // GET: /SavedJobs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SAVED_JOBS saved_jobs = db.SAVED_JOBS.Find(id);
            if (saved_jobs == null)
            {
                return HttpNotFound();
            }
            ViewBag.seeker_id = new SelectList(db.JOB_SEEKER_PROFILE, "seeker_id", "gender", saved_jobs.seeker_id);
            ViewBag.job_id = new SelectList(db.JOBS, "job_id", "job_title", saved_jobs.job_id);
            return View(saved_jobs);
        }

        // POST: /SavedJobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="seeker_id,job_id,saved_at")] SAVED_JOBS saved_jobs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(saved_jobs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.seeker_id = new SelectList(db.JOB_SEEKER_PROFILE, "seeker_id", "gender", saved_jobs.seeker_id);
            ViewBag.job_id = new SelectList(db.JOBS, "job_id", "job_title", saved_jobs.job_id);
            return View(saved_jobs);
        }

        // GET: /SavedJobs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SAVED_JOBS saved_jobs = db.SAVED_JOBS.Find(id);
            if (saved_jobs == null)
            {
                return HttpNotFound();
            }
            return View(saved_jobs);
        }

        // POST: /SavedJobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SAVED_JOBS saved_jobs = db.SAVED_JOBS.Find(id);
            db.SAVED_JOBS.Remove(saved_jobs);
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
