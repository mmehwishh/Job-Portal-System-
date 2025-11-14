using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Job_Portal_Managment_System.Controllers
{
    public class AccountController : Controller
    {
        private JobDBEntities db = new JobDBEntities();
        //
        // GET: /Account/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email , string password)
        {
            var user = db.USERS.FirstOrDefault(u => (u.email == email || u.username == email) && u.password_hash == password);
            if(user!=null)
            {
                Session["UserId"] = user.UserId;
                Session["UserName"] = user.username;

                System.Web.Security.FormsAuthentication.SetAuthCookie(user.username, false);


                if (user.role == "Employer")
                {
                    return RedirectToAction("Dashboard", "Employeer");
                }
                //else if (user.role == "Job Seeker")
                //{
                  //  return RedirectToAction("Dashboard", "JobSeeker");
                //}
                else
                    return RedirectToAction("Dashboard", "JobSeeker");
            }

            ViewBag.Error = "Invalid Email or password!";
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(USER user , string confirmPassword)
        {
            if(string.IsNullOrWhiteSpace(user.username)||
                string.IsNullOrWhiteSpace(user.email)||
                string.IsNullOrWhiteSpace(user.password_hash)||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                ViewBag.Error = "Please fill in all the required fields.";
                return View();
            }

            if(user.password_hash != confirmPassword)
            {
                ViewBag.Error = "Password do not match.";
                return View();
            }

            var existUser = db.USERS.FirstOrDefault(u=> u.username==user.username);
            if(existUser != null)
            {
                ViewBag.Error = "Username already exist.";
                return View();
            }

           // user.password_hash = BCrypt.Net.BCrypt.HashPassword(user.password_hash);

            user.created_at = DateTime.Now;
            if(string.IsNullOrWhiteSpace(user.role))
            {
                user.role = "Job Seeker";
            }

            db.USERS.Add(user);
            db.SaveChanges();

            return RedirectToAction("Login", "Account");
        }
	}
}