using CollegeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollegeManagementSystem.Controllers
{
    public class FacultyController : Controller
    {
        // GET: Faculty
        public ActionResult Profile()
        {
            var name = Convert.ToString(Session["name"]);
            //FacultyModels faculty = new FacultyModels();
            //try
            //{

            //    using (var db = new ApplicationDbContext())
            //    {
            //        faculty = db.Faculty.Join(db.Departments, F => F.Department, D => D.Id, (F, D) => new { F, D }).Where(w => w.F.IsActive == true && w.F.FacultyName == name).Select(s => new FacultyModels
            //        {
            //            Id = s.F.Id,
            //            Address = s.F.Address,
            //            JoiningDate = s.F.JoiningDate,
            //            ContactInfo = s.F.ContactInfo,
            //            Department = s.F.Department,
            //            Gender = s.F.Gender,
            //            DateOfBirth = s.F.DateOfBirth,
            //            FacultyName = s.F.FacultyName


            //        }).FirstOrDefault();
            //    }
            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}
            return View();
        }
    }
}