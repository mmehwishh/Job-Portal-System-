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
    public class JobSeekerController : Controller
    {
        private JobDBEntities db = new JobDBEntities();

        // GET: /JobSeeker/
        public ActionResult Index()
        {
            var job_seeker_profile = db.JOB_SEEKER_PROFILE.Include(j => j.USER);
            return View(job_seeker_profile.ToList());
        }

        // GET: /JobSeeker/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JOB_SEEKER_PROFILE job_seeker_profile = db.JOB_SEEKER_PROFILE.Find(id);
            if (job_seeker_profile == null)
            {
                return HttpNotFound();
            }
            return View(job_seeker_profile);
        }

        // GET: /JobSeeker/Create
        public ActionResult Create()
        {
            ViewBag.seeker_id = new SelectList(db.USERS, "UserId", "full_name");
            return View();
        }

        // POST: /JobSeeker/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="seeker_id,dob,gender,phone,address,experience_level,education,resume_file,ProfilePicturePath,Bio")] JOB_SEEKER_PROFILE job_seeker_profile)
        {
            if (ModelState.IsValid)
            {
                db.JOB_SEEKER_PROFILE.Add(job_seeker_profile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.seeker_id = new SelectList(db.USERS, "UserId", "full_name", job_seeker_profile.seeker_id);
            return View(job_seeker_profile);
        }

        // GET: /JobSeeker/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JOB_SEEKER_PROFILE job_seeker_profile = db.JOB_SEEKER_PROFILE.Find(id);
            if (job_seeker_profile == null)
            {
                return HttpNotFound();
            }
            ViewBag.seeker_id = new SelectList(db.USERS, "UserId", "full_name", job_seeker_profile.seeker_id);
            return View(job_seeker_profile);
        }

        // POST: /JobSeeker/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="seeker_id,dob,gender,phone,address,experience_level,education,resume_file,ProfilePicturePath,Bio")] JOB_SEEKER_PROFILE job_seeker_profile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job_seeker_profile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.seeker_id = new SelectList(db.USERS, "UserId", "full_name", job_seeker_profile.seeker_id);
            return View(job_seeker_profile);
        }

        // GET: /JobSeeker/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JOB_SEEKER_PROFILE job_seeker_profile = db.JOB_SEEKER_PROFILE.Find(id);
            if (job_seeker_profile == null)
            {
                return HttpNotFound();
            }
            return View(job_seeker_profile);
        }

        // POST: /JobSeeker/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JOB_SEEKER_PROFILE job_seeker_profile = db.JOB_SEEKER_PROFILE.Find(id);
            db.JOB_SEEKER_PROFILE.Remove(job_seeker_profile);
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
