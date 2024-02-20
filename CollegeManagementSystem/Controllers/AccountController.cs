using CollegeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollegeManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        // GET: Login
        public ActionResult UserLogin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ApplicationDbContext())
                {
                    var user = db.Users.Join(db.UserRole, U => U.Id, UR => UR.UserId, (U, UR) => new { U, UR })
                    .Join(db.Roles, U_UR => U_UR.UR.RoleId, R => R.Id, (U_UR, R) => new { U_UR, R })
                    .Join(db.UserDepartment, U_UR_R => U_UR_R.U_UR.U.Id, UD => UD.UserId, (U_UR_R, UD) => new { U_UR_R, UD })
                    .Join(db.Departments, U_UR_R_UD => U_UR_R_UD.UD.DeptId, D => D.Id, (U_UR_R_UD, D) => new { U_UR_R_UD, D })
                    .Where(w => w.U_UR_R_UD.U_UR_R.U_UR.U.Username == model.Username && w.U_UR_R_UD.U_UR_R.U_UR.U.Password == model.Password)
                    .Select(s => new UserModel
                    {
                        Id = s.U_UR_R_UD.U_UR_R.U_UR.U.Id,
                        Name=s.U_UR_R_UD.U_UR_R.U_UR.U.Name,
                        Password=s.U_UR_R_UD.U_UR_R.U_UR.U.Password,

                        PhoneNo = s.U_UR_R_UD.U_UR_R.U_UR.U.PhoneNo,
                        Username = s.U_UR_R_UD.U_UR_R.U_UR.U.Username,
                        DeptId = s.U_UR_R_UD.UD.DeptId,
                        DeptName = s.D.DeptName,
                        Role = s.U_UR_R_UD.U_UR_R.R.RoleName,
                        RoleId = s.U_UR_R_UD.U_UR_R.U_UR.UR.RoleId,
                        

                    }).FirstOrDefault();

                    if (user != null)
                    {
                        if (user.Role == "Admin")
                        {
                            Session["name"] = user.Name;
                            Session["Role"] = user.Role;
                            Session["id"] = user.Id;

                            return RedirectToAction("Dashboard", "Admin"); // Redirect to the Admin dashboard.
                        }
                        else if (user.Role == "Student")
                        {
                            Session["name"] = user.Name;
                            Session["id"] = user.Id;
                            Session["Role"] = user.Role;

                            return RedirectToAction("profile", "Student"); // Redirect to the Student dashboard.
                        }
                        else if (user.Role == "Faculty")
                        {
                            Session["name"] = user.Name;
                            Session["id"] = user.Id;
                            Session["Role"] = user.Role;

                            return RedirectToAction("profile", "Faculty"); // Redirect to the Student dashboard.
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                    }
                }
            }

            return View(model);
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            return View("UserLogin");
        }

    }
}