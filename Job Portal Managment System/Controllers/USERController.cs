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
    public class USERController : Controller
    {
        private JobDBEntities db = new JobDBEntities();

        // GET: /USER/
        public ActionResult Index()
        {
            var users = db.USERS.Include(u => u.EMPLOYER).Include(u => u.JOB_SEEKER_PROFILE);
            return View(users.ToList());
        }

        // GET: /USER/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER user = db.USERS.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: /USER/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.EMPLOYERS, "employer_id", "company_name");
            ViewBag.UserId = new SelectList(db.JOB_SEEKER_PROFILE, "seeker_id", "gender");
            return View();
        }

        // POST: /USER/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="full_name,email,password_hash,role,created_at,username,UserId")] USER user)
        {
            if (ModelState.IsValid)
            {
                db.USERS.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.EMPLOYERS, "employer_id", "company_name", user.UserId);
            ViewBag.UserId = new SelectList(db.JOB_SEEKER_PROFILE, "seeker_id", "gender", user.UserId);
            return View(user);
        }

        // GET: /USER/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER user = db.USERS.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.EMPLOYERS, "employer_id", "company_name", user.UserId);
            ViewBag.UserId = new SelectList(db.JOB_SEEKER_PROFILE, "seeker_id", "gender", user.UserId);
            return View(user);
        }

        // POST: /USER/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="full_name,email,password_hash,role,created_at,username,UserId")] USER user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.EMPLOYERS, "employer_id", "company_name", user.UserId);
            ViewBag.UserId = new SelectList(db.JOB_SEEKER_PROFILE, "seeker_id", "gender", user.UserId);
            return View(user);
        }

        // GET: /USER/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER user = db.USERS.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /USER/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            USER user = db.USERS.Find(id);
            db.USERS.Remove(user);
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
