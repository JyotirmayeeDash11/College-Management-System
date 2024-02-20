using CollegeManagementSystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace CollegeManagementSystem.Models
{

    public class AppModel
    {
        public static List<SelectListItem> ROLEselectListItems(bool Islogin = false)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (var dbContext = new ApplicationDbContext())
            {
                items = dbContext.Roles.OrderBy(o => o.RoleName).Select(r => new SelectListItem { Text = r.RoleName, Value = r.Id.ToString() }).ToList();
            }
            items.Insert(0, new SelectListItem { Text = "- Select Role -", Value = "" });
            return items;
        }

        public static List<SelectListItem> DeptSelectListItems()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (var dbContext = new ApplicationDbContext())
            {
                items = dbContext.Departments.Where(r => r.IsActive == true).OrderBy(o => o.DeptName).Select(r => new SelectListItem { Text = r.DeptName, Value = r.Id.ToString() }).ToList();
            }
            items.Insert(0, new SelectListItem { Text = "- Select Department -", Value = "" });
           // items.Insert(1, new SelectListItem { Text = "ALL", Value = "ALL" });
            return items;
        }



        public static List<SelectListItem> CourseSelectListItems()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (var dbContext = new ApplicationDbContext())
            {
               // items = dbContext.Courses.Where(r => r.IsActive == true && r.DeptId == deptId).OrderBy(o => o.CourseName).Select(r => new SelectListItem { Text = r.CourseName, Value = r.Id.ToString() }).ToList();
                items = dbContext.Courses.Where(r => r.IsActive == true).OrderBy(o => o.CourseName).Select(r => new SelectListItem { Text = r.CourseName, Value = r.Id.ToString() }).ToList();
            }
            items.Insert(0, new SelectListItem { Text = "- Select Courses -", Value = "" });
            // items.Insert(1, new SelectListItem { Text = "ALL", Value = "ALL" });
            return items;
        }


        public static UserModel GetUser(string id)
        {
           UserModel model = new UserModel();
            try
            {
                int userId=0;
                if (!string.IsNullOrEmpty(id)) 
                {
                    userId=Convert.ToInt32(id);
                }

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    model = db.Users.Join(db.UserRole, U => U.Id, UR => UR.UserId, (U, UR) => new { U, UR })
                    .Join(db.Roles, U_UR => U_UR.UR.RoleId, R => R.Id, (U_UR, R) => new { U_UR, R })
                    .Join(db.UserDepartment, U_UR_R => U_UR_R.U_UR.U.Id, UD => UD.UserId, (U_UR_R, UD) => new { U_UR_R, UD })
                    .Join(db.Departments, U_UR_R_UD => U_UR_R_UD.UD.DeptId, D => D.Id, (U_UR_R_UD, D) => new { U_UR_R_UD, D })
                    .Where(w => w.U_UR_R_UD.U_UR_R.U_UR.U.IsActive && w.U_UR_R_UD.U_UR_R.U_UR.U.Id== userId)
                    .Select(s => new UserModel
                    {
                        Id = s.U_UR_R_UD.U_UR_R.U_UR.U.Id,
                        Name = s.U_UR_R_UD.U_UR_R.U_UR.U.Name,
                        PhoneNo = s.U_UR_R_UD.U_UR_R.U_UR.U.PhoneNo,
                        Username = s.U_UR_R_UD.U_UR_R.U_UR.U.Username,
                        DeptId = s.U_UR_R_UD.UD.DeptId,
                        DeptName = s.D.DeptName,
                        Role = s.U_UR_R_UD.U_UR_R.R.RoleName,
                        RoleId = s.U_UR_R_UD.U_UR_R.U_UR.UR.RoleId,
                        
                    }).FirstOrDefault();

                    if (model.Role=="Student")
                    {
                        var _student = db.Students.Where(x => x.UserId == userId).FirstOrDefault();
                        if (_student!=null) 
                        {
                            model.StudentDOB = _student.DateOfBirth;
                            model.Gender = _student.Gender;
                            model.Address = _student.Address;
                            model.AdmissionDate = _student.AdmissionDate;
                            model.CourseEnrolled = _student.CourseEnrolled;

                        }

                    }
                    else if (model.Role == "Faculty")
                    {
                        var _faculty = db.Faculty.Where( x => x.UserId == userId).FirstOrDefault();
                        if (_faculty!=null)
                        {
                            model.FacultyDOB = _faculty.DateOfBirth;
                            model.FacultyAddress = _faculty.Address;
                            model.JoiningDate = _faculty.JoiningDate;
                            model.Gender = _faculty.Gender;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

    }

    public class ApiResult
    {
        public bool IsSuccessful { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
    }
    public class ApiResult<T>
    {
        public bool IsSuccessful { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }

}