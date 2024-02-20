
using CollegeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer;

namespace CollegeManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Dashboard()
        {
            List<totalcount> tc = new List<totalcount>();
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    int a = db.Courses.Where(w => w.IsActive == true).Count();
                    int b = db.Faculty.Where(w => w.IsActive == true).Count();
                    int c = db.Students.Where(w => w.IsActive == true).Count();

                    tc.Add(new totalcount
                    {
                        totalcourse = a,
                        totalstudent = c,
                        totalFaculty = b,
                    });

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return View(tc);
        }
        //public ActionResult RegisterStudents()
        //{
        //    return V iew();
        //}

        ApiResult apiResult = new ApiResult();
        // GET: Users
        public ActionResult Index()
        {
            List<UserModel> model = new List<UserModel>();
            try
            {

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    model = db.Users.Join(db.UserRole, U => U.Id, UR => UR.UserId, (U, UR) => new { U, UR })
                    .Join(db.Roles, U_UR => U_UR.UR.RoleId, R => R.Id, (U_UR, R) => new { U_UR, R })
                    .Join(db.UserDepartment, U_UR_R => U_UR_R.U_UR.U.Id, UD => UD.UserId, (U_UR_R, UD) => new { U_UR_R, UD })
                    .Join(db.Departments, U_UR_R_UD => U_UR_R_UD.UD.DeptId, D => D.Id, (U_UR_R_UD, D) => new { U_UR_R_UD, D })
                    .Where(w => w.U_UR_R_UD.U_UR_R.U_UR.U.IsActive)
                    .Select(s => new UserModel
                    {
                        Id = s.U_UR_R_UD.U_UR_R.U_UR.U.Id,
                        Name = s.U_UR_R_UD.U_UR_R.U_UR.U.Name,
                        PhoneNo = s.U_UR_R_UD.U_UR_R.U_UR.U.PhoneNo,
                        Username = s.U_UR_R_UD.U_UR_R.U_UR.U.Username,
                        DeptId = s.U_UR_R_UD.UD.DeptId,
                        DeptName = s.D.DeptName,
                        Role = s.U_UR_R_UD.U_UR_R.R.RoleName,
                        RoleId = s.U_UR_R_UD.U_UR_R.U_UR.UR.RoleId
                    }).ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(model);
        }

        // GET: User/Details/5
        //public ActionResult Details(string uID)
        //{
        //    UserProfile user = new UserProfile();
        //    if (!string.IsNullOrEmpty(uID))
        //    {
        //        user = App.GetUser(uID);
        //    }
        //    return View(user);
        //}
        // GET: User/Create
        public ActionResult Create()
        {
            ViewData["ROLEselectListItems"] = AppModel.ROLEselectListItems();
            ViewData["DeptSelectListItems"] = AppModel.DeptSelectListItems();
            ViewData["CourseSelectListItems"] = AppModel.CourseSelectListItems();
            var user = new UserModel();
            return View(user);
        }
        // POST: User/Create

        [HttpPost]
        public JsonResult CreateUser(UserModel user)
        {
            ApiResult apiResult = new ApiResult();
            int status = 0;

            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Users _user = new Users();
                    _user.Name = user.Name;
                    _user.Password = user.Password;
                    _user.PhoneNo = user.PhoneNo;
                    _user.Username = user.Username;
                    _user.CreatedBy = "Admin";
                    _user.CreatedOn = DateTime.Now;
                    _user.IsActive = true;
                    db.Users.Add(_user);
                    int i = db.SaveChanges();
                    if (i > 0)
                    {
                        UserRole userRole = new UserRole();
                        userRole.RoleId = user.RoleId;
                        userRole.UserId = _user.Id;
                        db.UserRole.Add(userRole);
                        db.SaveChanges();

                        UserDepartment userDepartment = new UserDepartment();
                        userDepartment.DeptId = user.DeptId;
                        userDepartment.UserId = _user.Id;
                        db.UserDepartment.Add(userDepartment);
                        db.SaveChanges();



                        if (user.Role == "Student")
                        {
                            Students _students = new Students();
                            _students.StudentName = user.Name;
                            _students.UserId = _user.Id;
                            _students.DateOfBirth = user.StudentDOB;
                            _students.Address = user.Address;
                            _students.AdmissionDate = user.AdmissionDate;
                            //_students.ContactInfo = user.ContactInfo;
                            _students.Gender = user.Gender;
                            _students.CourseEnrolled = user.CourseEnrolled;
                            _students.CreatedBy = "Admin";
                            _students.CreatedOn = DateTime.Now;
                            _students.IsActive = true;
                            db.Students.Add(_students);
                            status = db.SaveChanges();

                        }
                        else if (user.Role == "Faculty")
                        {
                            Faculty _faculty = new Faculty();
                            _faculty.FacultyName = user.Name;
                            _faculty.UserId = _user.Id;

                            _faculty.DateOfBirth = user.FacultyDOB;
                            _faculty.Gender = user.FacultyGender;
                            _faculty.Address = user.FacultyAddress;
                            _faculty.JoiningDate = user.JoiningDate;
                            //_faculty.ContactInfo = user.PhoneNo;
                            _faculty.CreatedBy = "Admin";
                            _faculty.CreatedOn = DateTime.Now;
                            _faculty.IsActive = true;
                            db.Faculty.Add(_faculty);
                            status = db.SaveChanges();
                        }

                    }
                    if (status > 0)
                    {

                        apiResult.IsSuccessful = true;
                        apiResult.Message = "User Account has been created.";

                    }
                    else
                    {
                        apiResult.IsSuccessful = false;
                        apiResult.Message = "Failed to create User Account. Please try again.";
                    }
                }
            }

            catch (Exception ex)
            {
                apiResult.IsSuccessful = false;
                apiResult.Message = "Failed to create User Account." + ex.Message;
            }

            return Json(apiResult);
        }
        //// GET: User/Edit/5
        [HttpGet]
        public ActionResult Edit(string uID)
        {
            var User = new UserModel();
            try
            {
                if (string.IsNullOrEmpty(uID))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                User = AppModel.GetUser(uID);
                ViewData["ROLEselectListItems"] = AppModel.ROLEselectListItems();
                ViewData["DeptSelectListItems"] = AppModel.DeptSelectListItems();
                ViewData["CourseSelectListItems"] = AppModel.CourseSelectListItems();
                //ViewData["VALIDUSERselectListItems"] = AppModel.VALIDUSERselectListItems();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(User);
        }
        [HttpPost]
        public JsonResult EditUser(UserModel user)
        {

            ApiResult apiResult = new ApiResult();
            int status = 0;

            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Users _user = db.Users.Where(x => x.IsActive && x.Id == user.Id).FirstOrDefault();
                    _user.Name = user.Name;
                    _user.PhoneNo = user.PhoneNo;
                    _user.Username = user.Username;
                    _user.UpdatedBy = "Admin";
                    _user.UpdatedOn = DateTime.Now;
                    int i = db.SaveChanges();
                    if (i > 0)
                    {

                        var _userRole = db.UserRole.Where(x => x.UserId == user.Id).FirstOrDefault();
                        var _userDept = db.UserDepartment.Where(x => x.UserId == user.Id).FirstOrDefault();
                        Students std = new Students();
                        Faculty fclt = new Faculty();

                        if (_userRole != null)
                        {
                            var _Role = db.Roles.Where(r => r.Id == _userRole.RoleId).FirstOrDefault();
                            if (_Role.RoleName == "Student")
                            {
                                var studDetls = db.Students.Where(s => s.IsActive && s.UserId == user.Id).FirstOrDefault();
                                if (studDetls != null)
                                {
                                    studDetls.StudentName = user.Name;
                                    studDetls.Address = user.Address;
                                    studDetls.AdmissionDate = user.AdmissionDate;
                                    studDetls.CourseEnrolled = user.CourseEnrolled;
                                    studDetls.Gender = user.Gender;
                                    studDetls.UpdatedOn=DateTime.Now;
                                    studDetls.UpdatedBy = "Admin";
                                    //studDetls.ContactInfo = user.ContactInfo;
                                    db.SaveChanges();

                                   
                                }
                                else
                                {
                                    Students stdDetls = new Students();
                                    stdDetls.StudentName = user.Name;
                                    stdDetls.Address = user.Address;
                                    stdDetls.AdmissionDate = user.AdmissionDate;
                                    stdDetls.CourseEnrolled = user.CourseEnrolled;
                                    stdDetls.Gender = user.Gender;
                                    stdDetls.CreatedOn = DateTime.Now;
                                    stdDetls.CreatedBy = "Admin";
                                    stdDetls.IsActive = true;
                                    //stdDetls.ContactInfo = user.ContactInfo;
                                    db.Students.Add(stdDetls);
                                    db.SaveChanges();


                                }
                            }
                            else if (_Role.RoleName == "Faculty")
                            {
                                var fcltyDetls = db.Faculty.Where(s => s.IsActive && s.UserId == user.Id).FirstOrDefault();
                                if (fcltyDetls != null)
                                {
                                    fcltyDetls.FacultyName = user.Name;
                                    fcltyDetls.Address = user.FacultyAddress;
                                    fcltyDetls.JoiningDate = user.JoiningDate;
                                    fcltyDetls.DateOfBirth = user.FacultyDOB;
                                    fcltyDetls.Gender = user.Gender;
                                    fcltyDetls.Department=user.DeptId; 
                                    fcltyDetls.UpdatedOn = DateTime.Now;
                                    fcltyDetls.UpdatedBy = "Admin";
                                    //fcltyDetls.ContactInfo = user.ContactInfo;
                                    db.SaveChanges();


                                }
                                else
                                {
                                    Faculty fclDetls = new Faculty();
                                    fclDetls.FacultyName = user.FacultyName;
                                    fclDetls.Address = user.FacultyAddress;
                                    fclDetls.JoiningDate = user.JoiningDate;
                                    fclDetls.DateOfBirth = user.FacultyDOB;
                                    fclDetls.Gender = user.Gender;
                                    fclDetls.CreatedOn = DateTime.Now;
                                    fclDetls.CreatedBy = "Admin";
                                    fclDetls.IsActive = true;

                                    //fcltyDetls.ContactInfo = user.ContactInfo;
                                    db.Faculty.Add(fclDetls);
                                    db.SaveChanges();

                                }
                            }


                            _userRole.RoleId = user.RoleId;
                            _userRole.UserId = _user.Id;
                            db.SaveChanges();


                        }
                        else
                        {

                            UserRole userRole = new UserRole();
                            userRole.RoleId = user.RoleId;
                            userRole.UserId = _user.Id;
                            db.UserRole.Add(userRole);

                            db.SaveChanges();


                            var _Role = db.Roles.Where(r => r.Id == userRole.RoleId).FirstOrDefault();
                            if (_Role.RoleName == "Student")
                            {

                                Students stdDetls = new Students();
                                stdDetls.StudentName = user.Name;
                                stdDetls.Address = user.Address;
                                stdDetls.AdmissionDate = user.AdmissionDate;
                                stdDetls.CourseEnrolled = user.CourseEnrolled;
                                stdDetls.Gender = user.Gender;
                                stdDetls.CreatedBy = "Admin";
                                stdDetls.CreatedOn = DateTime.Now;
                                //stdDetls.ContactInfo = user.ContactInfo;
                                db.Students.Add(stdDetls);
                                db.SaveChanges();


                            }
                            else if (_Role.RoleName == "Faculty")
                            {

                                Faculty fclDetls = new Faculty();
                                fclDetls.FacultyName = user.Name;
                                fclDetls.Address = user.Address;
                                fclDetls.JoiningDate = user.JoiningDate;
                                fclDetls.DateOfBirth = user.FacultyDOB;
                                fclDetls.Gender = user.Gender;
                                fclDetls.CreatedBy = "Admin";
                                fclDetls.CreatedOn = DateTime.Now;
                                //fclDetls.ContactInfo = user.ContactInfo;
                                db.Faculty.Add(fclDetls);
                                db.SaveChanges();

                            }
                        }

                        if (_userDept != null)
                        {
                            _userDept.DeptId = user.DeptId;
                            _userDept.UserId = _user.Id;
                            db.SaveChanges();

                        }
                        else
                        {
                            UserDepartment userDept = new UserDepartment();
                            userDept.DeptId = user.RoleId;
                            userDept.UserId = _user.Id;
                            db.UserDepartment.Add(userDept);

                        }
                        status = 1;
                    }
                    if (status > 0)
                    {

                        apiResult.IsSuccessful = true;
                        apiResult.Message = "User Account has been Updated.";

                    }
                    else
                    {
                        apiResult.IsSuccessful = false;
                        apiResult.Message = "Failed to Update User Account. Please try again.";
                    }
                }
            }

            catch (Exception ex)
            {
                apiResult.IsSuccessful = false;
                apiResult.Message = "Failed to Update User Account." + ex.Message;
            }

            return Json(apiResult);
        }
        // GET: User/Delete/5
        public ActionResult Delete(string uID)
        {
            if (string.IsNullOrEmpty(uID))
            {
                return RedirectToAction("Index");
            }
            try
            {
                int userId = Convert.ToInt32(uID);
                ApplicationDbContext db = new ApplicationDbContext();
                var _user = db.Users.Where(u => u.IsActive && u.Id == userId).FirstOrDefault();
                if (_user != null)
                {
                    _user.IsActive = false;
                    _user.UpdatedOn = DateTime.Now;
                    _user.UpdatedBy = "Admin";
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Index");
        }





        

        //[HttpPost]
        //public JsonResult ResetPassword(string userId, string password)
        //{
        //    ApiResult apiResult = new ApiResult();
        //    try
        //    {
        //        ApplicationDbContext context = new ApplicationDbContext();
        //        ApplicationUserManager userManager = new ApplicationUserManager(new ApplicationUserStore(context));
        //        var provider = new DpapiDataProtectionProvider(Helpers.ApplicationName);
        //        userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(provider.Create("ASP.NET Identity"));
        //        var u = userManager.FindById(userId);
        //        string email = u.Email;
        //        if (!string.IsNullOrEmpty(email))
        //        {
        //            string role = userManager.GetRoles(u.Id).FirstOrDefault();
        //            string token = userManager.GeneratePasswordResetToken(userId);
        //            IdentityResult result = userManager.ResetPassword(userId, token, password);
        //            if (result.Succeeded)
        //            {
        //                if (Helpers.TriggerEmail)
        //                {
        //                    apiResult.Message = "Password has been modified";
        //                }
        //                else
        //                {
        //                    apiResult.IsSuccessful = true;
        //                    apiResult.Message = "Password has been modified";
        //                }
        //            }
        //            else
        //            {
        //                apiResult.IsSuccessful = false;
        //                apiResult.Message = string.Join("", result.Errors);
        //            }
        //        }
        //        else
        //        {
        //            apiResult.IsSuccessful = false;
        //            apiResult.Message = "Unable to reset Password due to not avilable Email Address!!!";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (Helpers.WriteExceptionsToEventLog)
        //        { Helpers.WriteToErrorLog(ex, "ResetPassword(userId,password)"); }
        //        else
        //        { throw ex; }
        //    }
        //    return Json(apiResult);
        //}

        //[HttpPost]
        //public JsonResult GeneratePassword()
        //{
        //    int lengthOfPassword = 6;
        //    string passkey = "abcdefghijklmnozABCDEFGHIJKLMNOZ1234567890";
        //    StringBuilder strB = new StringBuilder(100);
        //    Random random = new Random();
        //    while (0 < lengthOfPassword--)
        //    {
        //        strB.Append(passkey[random.Next(passkey.Length)]);
        //    }
        //    return Json(new { Result = "success", Password = strB.ToString() });
        //}








        //[HttpPost]
        //. public ActionResult RegisterStudents(Students student)
        //{
        //    try
        //    {
        //        using (CMS_dbEntities db = new CMS_dbEntities())
        //        {
        //            student.IsActive = true;

        //            student.CreatedBy = "Admin";
        //            student.CreatedOn = DateTime.Now;

        //            db.Students.Add(student);
        //            db.SaveChanges();

        //            // Return a success message
        //            return Json(new { success = true });
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        // Return an error message
        //        return Json(new { success = false, message = "Registration failed." });
        //    }
        //}


        //public ActionResult ViewStudentManagement()
        //{
        //    List<StudentModel> student = new List<StudentModel>();
        //    using (CMS_dbEntities db = new CMS_dbEntities())
        //    {
        //        // Retrieve the list of registered students
        //        student = db.Students.Where(s => s.IsActive == true).Select(s => new StudentModel
        //        {
        //            Id = s.Id,
        //            StudentName = s.StudentName,
        //            DateOfBirth = s.DateOfBirth,
        //            Address = s.Address,
        //            Gender = s.Gender,
        //            ContactInfo = s.ContactInfo,
        //            AdmissionDate = s.AdmissionDate,
        //            CourseEnrolled = s.CourseEnrolled
        //        }).ToList();
        //    }

        //    return View(student);
        //}
        //public ActionResult EditStudents(int id)
        //{
        //    StudentModel student = new StudentModel();
        //    try
        //    {
        //        using (CMS_dbEntities db = new CMS_dbEntities())
        //        {
        //            student = db.Students.Where(w => w.IsActive == true && w.Id == id).Select(s => new StudentModel
        //            {
        //                StudentName = s.StudentName,
        //                DateOfBirth = s.DateOfBirth,
        //                ContactInfo = s.ContactInfo,
        //                Gender = s.Gender,
        //                Address = s.Address,
        //                AdmissionDate = s.AdmissionDate,
        //                CourseEnrolled = s.CourseEnrolled

        //            }).FirstOrDefault();
        //            return View(student);
        //        }
        //    }




        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //}
        //[HttpPost]
        //public ActionResult SetStudent(Students student)
        //{
        //    try
        //    {
        //        using (CMS_dbEntities db = new CMS_dbEntities())
        //        {
        //            var studentdetails = db.Students.Where(w => w.IsActive == true && w.Id == student.Id).FirstOrDefault();
        //            if (studentdetails != null)
        //            {
        //                studentdetails.StudentName = student.StudentName;
        //                studentdetails.DateOfBirth = student.DateOfBirth;
        //                studentdetails.Gender = student.Gender;
        //                studentdetails.ContactInfo = student.ContactInfo;
        //                studentdetails.Address = student.Address;
        //                studentdetails.AdmissionDate = student.AdmissionDate;
        //                studentdetails.CourseEnrolled = student.CourseEnrolled;
        //                studentdetails.UpdatedBy = "Admin";
        //                studentdetails.UpdatedOn = DateTime.Now;

        //                db.SaveChanges();


        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    return View("student");
        //}
    }
}