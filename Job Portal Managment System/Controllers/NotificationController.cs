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
    public class NotificationController : Controller
    {
        private JobDBEntities db = new JobDBEntities();

        // GET: /Notification/
        public ActionResult Index()
        {
            var notifications = db.NOTIFICATIONS.Include(n => n.USER);
            return View(notifications.ToList());
        }

        // GET: /Notification/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NOTIFICATION notification = db.NOTIFICATIONS.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            return View(notification);
        }

        // GET: /Notification/Create
        public ActionResult Create()
        {
            ViewBag.user_id = new SelectList(db.USERS, "UserId", "full_name");
            return View();
        }

        // POST: /Notification/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="notification_id,user_id,message,created_at,read_status")] NOTIFICATION notification)
        {
            if (ModelState.IsValid)
            {
                db.NOTIFICATIONS.Add(notification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.user_id = new SelectList(db.USERS, "UserId", "full_name", notification.user_id);
            return View(notification);
        }

        // GET: /Notification/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NOTIFICATION notification = db.NOTIFICATIONS.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            ViewBag.user_id = new SelectList(db.USERS, "UserId", "full_name", notification.user_id);
            return View(notification);
        }

        // POST: /Notification/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="notification_id,user_id,message,created_at,read_status")] NOTIFICATION notification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(notification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.user_id = new SelectList(db.USERS, "UserId", "full_name", notification.user_id);
            return View(notification);
        }

        // GET: /Notification/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NOTIFICATION notification = db.NOTIFICATIONS.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            return View(notification);
        }

        // POST: /Notification/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NOTIFICATION notification = db.NOTIFICATIONS.Find(id);
            db.NOTIFICATIONS.Remove(notification);
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
