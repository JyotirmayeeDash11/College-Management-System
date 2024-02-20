using CollegeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollegeManagementSystem.Controllers
{
    public class AttendanceController : Controller
    {
        // GET: Attendance
        public ActionResult MarkAttendance()
        {
            using(var db = new ApplicationDbContext())
    {
                var viewModel = new AttendanceViewModel();

                
                viewModel.StudentList = db.Students
                    .Select(s => new SelectListItem
                    {
                        Text = s.StudentName,
                        Value = s.Id.ToString()
                    })
                    .ToList();

                viewModel.CourseList = db.Courses
                    .Select(c => new SelectListItem
                    {
                        Text = c.CourseName,
                        Value = c.Id.ToString()
                    })
                    .ToList();

                return View(viewModel);
            }
        }



        [HttpPost]
        public ActionResult MarkAttendance(AttendanceViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ApplicationDbContext())
                {
                    var attendance = new Attendance
                    {
                        StudentID = model.StudentID,
                        CourseID = model.CourseID,
                        AttendanceDate = model.AttendanceDate,
                        
                       
                    };

                    db.Attendance.Add(attendance);
                    db.SaveChanges();

                    return RedirectToAction("MarkAttendance", "Attendance"); 
                }
            }

          
            return View(model);
        }





    }
}