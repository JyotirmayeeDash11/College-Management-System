using CollegeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollegeManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Profile()
        {
            var name =Convert.ToString(Session["name"]);
            StudentModels std=new StudentModels();
            try
            {

                using (var db = new ApplicationDbContext())
                {
                    std=db.Students.Join(db.Courses, stud=> stud.CourseEnrolled,cs=> cs.Id,(stud,cs)=> new { stud, cs } ).Where(w => w.stud.IsActive == true && w.stud.StudentName == name ).Select(s=> new StudentModels
                    {
                        StudId = s.stud.Id,
                        Address = s.stud.Address,
                        AdmissionDate = s.stud.AdmissionDate,
                        ContactInfo = s.stud.ContactInfo,
                        CourseEnrolled = s.stud.CourseEnrolled,
                        Gender = s.stud.Gender,
                        StudentDOB = s.stud.DateOfBirth,
                        StudentName = s.stud.StudentName,
                        coursename=s.cs.CourseName,
                        
                    }).FirstOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View(std);
        }
    }
}