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
    public class UserSkillsController : Controller
    {
        private JobDBEntities db = new JobDBEntities();

        // GET: /UserSkills/
        public ActionResult Index()
        {
            var user_skills = db.USER_SKILLS.Include(u => u.JOB_SEEKER_PROFILE).Include(u => u.Skill);
            return View(user_skills.ToList());
        }

        // GET: /UserSkills/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER_SKILLS user_skills = db.USER_SKILLS.Find(id);
            if (user_skills == null)
            {
                return HttpNotFound();
            }
            return View(user_skills);
        }

        // GET: /UserSkills/Create
        public ActionResult Create()
        {
            ViewBag.seeker_id = new SelectList(db.JOB_SEEKER_PROFILE, "seeker_id", "gender");
            ViewBag.SkillID = new SelectList(db.Skills, "skillID", "skillName");
            return View();
        }

        // POST: /UserSkills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="JobSeekerSkillID,seeker_id,SkillID")] USER_SKILLS user_skills)
        {
            if (ModelState.IsValid)
            {
                db.USER_SKILLS.Add(user_skills);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.seeker_id = new SelectList(db.JOB_SEEKER_PROFILE, "seeker_id", "gender", user_skills.seeker_id);
            ViewBag.SkillID = new SelectList(db.Skills, "skillID", "skillName", user_skills.SkillID);
            return View(user_skills);
        }

        // GET: /UserSkills/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER_SKILLS user_skills = db.USER_SKILLS.Find(id);
            if (user_skills == null)
            {
                return HttpNotFound();
            }
            ViewBag.seeker_id = new SelectList(db.JOB_SEEKER_PROFILE, "seeker_id", "gender", user_skills.seeker_id);
            ViewBag.SkillID = new SelectList(db.Skills, "skillID", "skillName", user_skills.SkillID);
            return View(user_skills);
        }

        // POST: /UserSkills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="JobSeekerSkillID,seeker_id,SkillID")] USER_SKILLS user_skills)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user_skills).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.seeker_id = new SelectList(db.JOB_SEEKER_PROFILE, "seeker_id", "gender", user_skills.seeker_id);
            ViewBag.SkillID = new SelectList(db.Skills, "skillID", "skillName", user_skills.SkillID);
            return View(user_skills);
        }

        // GET: /UserSkills/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER_SKILLS user_skills = db.USER_SKILLS.Find(id);
            if (user_skills == null)
            {
                return HttpNotFound();
            }
            return View(user_skills);
        }

        // POST: /UserSkills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            USER_SKILLS user_skills = db.USER_SKILLS.Find(id);
            db.USER_SKILLS.Remove(user_skills);
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
