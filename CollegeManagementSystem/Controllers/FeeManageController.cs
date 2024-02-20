using CollegeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer;

namespace CollegeManagementSystem.Controllers
{
    public class FeeManageController : Controller
    {

        //List of Fee.

        public ActionResult Index()
        {
            List<FeeModel> master = new List<FeeModel>();
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    master = db.Fees.Join(db.Students, F => F.StudentID, S => S.Id, (F, S) => new { F, S })
                        .Join(db.Courses, F_S => F_S.F.CourseID, C => C.Id, (F_S, C) => new { F_S, C })
                        .Join(db.Departments, F_S_C => F_S_C.F_S.F.DeptId, D => D.Id, (F_S_C, D) => new { F_S_C, D })
                        .Where(X => X.F_S_C.F_S.F.IsActive)
                        .Select(SL => new FeeModel
                        {
                            CourseID = SL.F_S_C.F_S.F.StudentID,
                            CourseFee = SL.F_S_C.F_S.F.CourseID,
                            Course = SL.F_S_C.C.CreatedBy,
                            DeptId = SL.F_S_C.F_S.F.DeptId,
                            DeptName = SL.D.DeptName,
                            Id = SL.F_S_C.F_S.F.Id,
                            PaidAmount = SL.F_S_C.F_S.F.PaidAmount,
                            PaymentDate = SL.F_S_C.F_S.F.PaymentDate,
                            Student = SL.F_S_C.F_S.S.StudentName,
                            StudentID = SL.F_S_C.F_S.F.StudentID,
                        }).OrderBy(o => o.Student).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(master);
        }

        //Add Fee
        public ActionResult AddFee(string Id)
        {

            ViewData["DEPTselectListItems"] = AppModel.DeptSelectListItems();
            return PartialView("_AddFee", GetFeeDetails(Id));
        }

        [HttpPost]
        public JsonResult PostFee(FeeModel model)
        {

            ApiResult apiResult = new ApiResult();
            Fees _model = new Fees();
            int i = 0;
            if (ModelState.IsValid)
            {
                try
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        //var modelDetails = string.Empty;
                        var modelDetails = db.Fees.Where(x => x.Id == model.Id).FirstOrDefault();
                        if (modelDetails == null)
                        {
                            _model.DeptId = model.DeptId;
                            _model.CourseFee = model.CourseFee;
                            _model.CourseID = model.CourseID;
                            _model.StudentID = model.StudentID;
                            _model.PaidAmount = model.PaidAmount;
                            _model.CreatedBy = "Admin";
                            _model.CreatedOn = DateTime.Now;
                            _model.IsActive = true;
                            db.Fees.Add(_model);
                            i = db.SaveChanges();
                            if (i > 0)
                            {
                                apiResult.Data = null;
                                apiResult.IsSuccessful = true;
                                apiResult.Message = "Add successfully.";
                            }
                            else
                            {
                                apiResult.Data = null;
                                apiResult.IsSuccessful = true;
                                apiResult.Message = "Failed to add.";
                            }
                        }
                        else
                        {
                            apiResult.Data = null;
                            apiResult.IsSuccessful = false;
                            apiResult.Message = "Add already exists";
                        }
                    }

                }
                catch (Exception ex)
                {
                    apiResult.Data = null;
                    apiResult.IsSuccessful = false;
                    apiResult.Message = " Failed to map error. " + ex.Message;
                    throw ex;
                }
            }
            return Json(apiResult);
        }


        //Edit Fee
        public ActionResult EditFee(string Id)
        {
            return PartialView("_EditFee", GetFeeDetails(Id));
        }
        [HttpPost]
        public JsonResult UpdateMappingMaster(FeeModel model)
        {
            ApiResult apiResult = new ApiResult();
            Fees _model = new Fees();
            int i = 0;
            if (ModelState.IsValid)
            {
                try
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        _model = db.Fees.Find(model.Id);
                        if (_model != null)
                        {
                            _model.DeptId = model.DeptId;
                            _model.CourseID = model.CourseID;
                            _model.StudentID = model.StudentID;
                            _model.CourseFee = model.CourseFee;
                            _model.PaidAmount = model.PaidAmount;
                            _model.PaymentDate = model.PaymentDate;
                            _model.UpdatedBy = "ADMIN";
                            _model.UpdatedOn = DateTime.Now;
                            i = db.SaveChanges();
                            if (i > 0)
                            {
                                apiResult.Data = null;
                                apiResult.IsSuccessful = true;
                                apiResult.Message = "Updated successfully.";
                            }
                            else
                            {
                                apiResult.Data = null;
                                apiResult.IsSuccessful = false;
                                apiResult.Message = "Failed to update.";
                            }
                        }

                    }

                }
                catch (Exception ex)
                {
                    apiResult.Data = null;
                    apiResult.IsSuccessful = false;
                    apiResult.Message = "Failed to update. error. " + ex.Message;

                    throw ex;

                }
            }
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }



        //Id wise Fee detail.
        public Fees GetFeeDetails(string Id)
        {
            Fees model = new Fees();
            try
            {
                int fId;
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    if (!string.IsNullOrEmpty(Id))
                    {
                        fId = Convert.ToInt32(Id);
                        model = db.Fees.Where(x => x.Id == fId).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        [HttpPost]
        public JsonResult GetCourseByDepartment(string deptId)
        {
            ApiResult apiResult = new ApiResult();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<SelectListItem> items = new List<SelectListItem>();
                if (!string.IsNullOrEmpty(deptId))
                {
                    int depId = Convert.ToInt32(deptId);
                    var _model = db.Courses.Where(x => x.DeptId == depId && x.IsActive)
                    .OrderBy(o => o.CourseName).Distinct();
                    items = _model.Select(r => new SelectListItem { Text = r.CourseName, Value = r.Id.ToString() }).ToList();
                }
                if (items.Count > 0)
                {
                    apiResult.IsSuccessful = true;
                    apiResult.Data = items;
                }
                else
                {
                    apiResult.IsSuccessful = false;
                }
            }
            return Json(apiResult, JsonRequestBehavior.DenyGet);
        }



        [HttpPost]
        public JsonResult GetStudentByCourse(string courseId)
        {
            ApiResult apiResult = new ApiResult();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<SelectListItem> items = new List<SelectListItem>();
                if (!string.IsNullOrEmpty(courseId))
                {
                    int crsId = Convert.ToInt32(courseId);
                    var _model = db.Students.Where(x => x.CourseEnrolled == crsId && x.IsActive)
                    .OrderBy(o => o.StudentName).Distinct();
                    items = _model.Select(r => new SelectListItem { Text = r.StudentName, Value = r.Id.ToString() }).ToList();
                }
                if (items.Count > 0)
                {
                    apiResult.IsSuccessful = true;
                    apiResult.Data = items;
                }
                else
                {
                    apiResult.IsSuccessful = false;
                }
            }
            return Json(apiResult, JsonRequestBehavior.DenyGet);
        }



    }
}