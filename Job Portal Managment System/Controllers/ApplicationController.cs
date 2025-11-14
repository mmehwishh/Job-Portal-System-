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
    public class ApplicationController : Controller
    {
        private JobDBEntities db = new JobDBEntities();

        // GET: /Application/
        public ActionResult Index()
        {
            var applications = db.APPLICATIONS.Include(a => a.JOB).Include(a => a.JOB_SEEKER_PROFILE);
            return View(applications.ToList());
        }

        // GET: /Application/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APPLICATION application = db.APPLICATIONS.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        // GET: /Application/Create
        public ActionResult Create()
        {
            ViewBag.job_id = new SelectList(db.JOBS, "job_id", "job_title");
            ViewBag.seeker_id = new SelectList(db.JOB_SEEKER_PROFILE, "seeker_id", "gender");
            return View();
        }

        // POST: /Application/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="application_id,job_id,seeker_id,applied_date,status")] APPLICATION application)
        {
            if (ModelState.IsValid)
            {
                db.APPLICATIONS.Add(application);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.job_id = new SelectList(db.JOBS, "job_id", "job_title", application.job_id);
            ViewBag.seeker_id = new SelectList(db.JOB_SEEKER_PROFILE, "seeker_id", "gender", application.seeker_id);
            return View(application);
        }

        // GET: /Application/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APPLICATION application = db.APPLICATIONS.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            ViewBag.job_id = new SelectList(db.JOBS, "job_id", "job_title", application.job_id);
            ViewBag.seeker_id = new SelectList(db.JOB_SEEKER_PROFILE, "seeker_id", "gender", application.seeker_id);
            return View(application);
        }

        // POST: /Application/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="application_id,job_id,seeker_id,applied_date,status")] APPLICATION application)
        {
            if (ModelState.IsValid)
            {
                db.Entry(application).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.job_id = new SelectList(db.JOBS, "job_id", "job_title", application.job_id);
            ViewBag.seeker_id = new SelectList(db.JOB_SEEKER_PROFILE, "seeker_id", "gender", application.seeker_id);
            return View(application);
        }

        // GET: /Application/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APPLICATION application = db.APPLICATIONS.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        // POST: /Application/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            APPLICATION application = db.APPLICATIONS.Find(id);
            db.APPLICATIONS.Remove(application);
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
