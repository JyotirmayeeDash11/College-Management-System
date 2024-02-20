using CollegeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollegeManagementSystem.Controllers
{
    public class MasterController : Controller
    {
        // GET: Master

        //Departments
        ApiResult apiResult=new ApiResult();    
        public ActionResult ViewDepartments()
        {
            List<Departments> model = new List<Departments>();
            try
            {

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    model = db.Departments.Where(D => D.IsActive).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(model);
        }



        // GET: User/Create
        public ActionResult CreateDepartment()
        {
            var user = new Departments();
            return View(user);
        }
        // POST: User/Create
        [HttpPost]
        public JsonResult CreateDepartment(Departments departments)
        {
            try
            {
                ApplicationDbContext db = new ApplicationDbContext();
                Departments _department = new Departments();
                _department.DeptName = departments.DeptName;
                _department.DeptCode = departments.DeptCode;
                _department.DeptShortName = departments.DeptShortName;
                _department.CreatedBy = "Admin";
                _department.CreatedOn = DateTime.Now;
                _department.IsActive = true;
                db.Departments.Add(_department);
                int i = db.SaveChanges();
                if (i > 0)
                {
                    apiResult.IsSuccessful = true;
                    apiResult.Message = "Department has been created.";
                }
                else
                {
                    apiResult.IsSuccessful = false;
                    apiResult.Message = "Failed to create department.";
                }
            }
            catch (Exception ex)
            {
                apiResult.IsSuccessful = false;
                apiResult.Message = "Failed to create department";
                throw ex;


            }
            return Json(apiResult);
        }


        // GET: User/Edit/5
        [HttpGet]
        public ActionResult EditDepartment(string id)
        {
            var model = new Departments();

            try
            {
                int Id = Convert.ToInt32(id);
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    model = db.Departments.Where(D => D.Id == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(model);
        }
        [HttpPost]
        public JsonResult EditDepartment(Departments departments)
        {
            try
            {
                ApplicationDbContext db = new ApplicationDbContext();
                Departments _department = new Departments();
                int i = 0;
                _department = db.Departments.Where(x => x.Id == departments.Id).FirstOrDefault();
                if (_department != null)
                {
                    _department.DeptName = departments.DeptName;
                    _department.DeptCode = departments.DeptCode;
                    _department.DeptShortName = departments.DeptShortName;
                    _department.UpdatedBy = "Admin";
                    _department.UpdatedOn = DateTime.Now;
                    i = db.SaveChanges();
                }
                if (i > 0)
                {

                    apiResult.IsSuccessful = true;
                    apiResult.Message = "Department has been Updated.";
                }
                else
                {
                    apiResult.IsSuccessful = false;
                    apiResult.Message = "Failed to Update Department.";
                }

            }
            catch (Exception ex)
            {
                apiResult.IsSuccessful = false;
                apiResult.Message = "Failed to update department.";
                throw ex;


            }
            return Json(apiResult);

        }
        // GET: User/Delete/5
        public ActionResult DeleteDepartment(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {

                int dId = Convert.ToInt32(id);

                Departments _user = new Departments();
                ApplicationDbContext db = new ApplicationDbContext();
                _user = db.Departments.Where(x => x.Id == dId).FirstOrDefault();
                if (_user != null)
                {
                    _user.IsActive = false;
                    _user.UpdatedBy = "Admin";
                    _user.UpdatedOn = DateTime.Now;
                    db.SaveChanges();
                }

                return RedirectToAction("ViewDepartments");
            }

            return RedirectToAction("ViewDepartments");
        }


        //for courses

        public ActionResult ViewCourses()
        {
            List<CoursesModel> model = new List<CoursesModel>();
            try
            {

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    model = db.Courses.Join(db.Departments, C => C.DeptId, D => D.Id, (C, D) => new { C, D })
                        .Where(X => X.D.IsActive && X.C.IsActive)
                        .Select(s=>new CoursesModel
                        {
                            CourseDescription=s.C.CourseDescription,
                            CourseDuration=s.C.CourseDuration,
                            CourseFee=s.C.CourseFee,
                            CourseName=s.C.CourseName,
                            DeptId=s.C.DeptId,
                            DeptName=s.D.DeptName,
                            Id=s.C.Id,
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(model);
        }



        // GET: User/Create
        public ActionResult CreateCourse()
        {
            var course = new Courses();
            ViewData["DeptSelectListItems"] = AppModel.DeptSelectListItems();

            return View(course);
        }
        // POST: User/Create
        [HttpPost]
        public JsonResult CreateCourse(Courses courses)
        {
            try
            {
                ApplicationDbContext db = new ApplicationDbContext();
                Courses _courses = new Courses();
                _courses.CourseName = courses.CourseName;
                _courses.DeptId = courses.DeptId;
                _courses.CourseDescription = courses.CourseDescription;
                _courses.CourseDuration = courses.CourseDuration;
                _courses.CourseFee = courses.CourseFee;
                _courses.CreatedBy = "Admin";
                _courses.CreatedOn = DateTime.Now;
                _courses.IsActive = true;
                db.Courses.Add(_courses);
                int i = db.SaveChanges();
                if (i > 0)
                {
                    apiResult.IsSuccessful = true;
                    apiResult.Message = "Course has been created.";
                }
                else
                {
                    apiResult.IsSuccessful = false;
                    apiResult.Message = "Failed to create Course.";
                }
            }
            catch (Exception ex)
            {
                apiResult.IsSuccessful = false;
                apiResult.Message = "Failed to create Course";
                throw ex;


            }
            return Json(apiResult);
        }


        // GET: User/Edit/5
        [HttpGet]
        public ActionResult EditCourse(string id)
        {
            var model = new Courses();
            ViewData["DeptSelectListItems"] = AppModel.DeptSelectListItems();


            try
            {
                int Id = Convert.ToInt32(id);
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    model = db.Courses.Where(D => D.Id == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(model);
        }
        [HttpPost]
        public JsonResult EditCourse(Courses courses)
        {
            try
            {
                ApplicationDbContext db = new ApplicationDbContext();
                Courses _courses = new Courses();
                int i = 0;
                _courses = db.Courses.Where(x => x.Id == courses.Id).FirstOrDefault();
                if (_courses != null)
                {
                    _courses.CourseName = courses.CourseName;
                    _courses.DeptId = courses.DeptId;
                    _courses.CourseDescription = courses.CourseDescription;
                    _courses.CourseDuration = courses.CourseDuration;
                    _courses.CourseFee = courses.CourseFee;
                    
                    _courses.UpdatedBy = "Admin";
                    _courses.UpdatedOn = DateTime.Now;
                    i = db.SaveChanges();
                }
                if (i > 0)
                {

                    apiResult.IsSuccessful = true;
                    apiResult.Message = "Course has been Updated.";
                }
                else
                {
                    apiResult.IsSuccessful = false;
                    apiResult.Message = "Failed to Update Course.";
                }

            }
            catch (Exception ex)
            {
                apiResult.IsSuccessful = false;
                apiResult.Message = "Failed to update Course.";
                throw ex;


            }
            return Json(apiResult);

        }
        // GET: User/Delete/5
        public ActionResult DeleteCourse(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {

                int cId = Convert.ToInt32(id);

                Courses _course = new Courses();
                ApplicationDbContext db = new ApplicationDbContext();
                _course = db.Courses.Where(x => x.Id == cId).FirstOrDefault();
                if (_course != null)
                {
                    _course.IsActive = false;
                    _course.UpdatedBy = "Admin";
                    _course.UpdatedOn = DateTime.Now;
                    db.SaveChanges();
                }

                return RedirectToAction("ViewCourses");
            }

            return RedirectToAction("ViewCourses");
        }



    }
}