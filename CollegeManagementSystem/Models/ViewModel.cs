using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CollegeManagementSystem.Models
{

    //public class DepartmentModel
    //{
    //    public int Id { get; set; }
    //    public string DeptName { get; set; }
    //    public string DeptCode { get; set; }    
    //    public string Description { get; set; }
    //    public string PrefixName { get; set; }
    //    public bool IsActive { get; set; }

    //}


    public class UserProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string PhoneNo { get; set; }


        public int DeptId { get; set; }
        public string DeptName { get; set; }

        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public bool IsActive { get; set; }
    }

    public class totalcount
    {
        public int totalFaculty { get; set; }
        public int totalstudent { get; set; }
        public int totalcourse { get; set; }
    }
    public class CoursesModel
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public string CourseDescription { get; set; }
        public int CourseDuration { get; set; }
        public decimal CourseFee { get; set; }
    }

    public class FeeModel
    {
        public int Id { get; set; }
        public int StudentID { get; set; }
        public string Student { get; set; }
        public int CourseID { get; set; }
        public string Course { get; set; }
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public decimal CourseFee { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime PaymentDate { get; set; }

    }
    public class StudentModels
    {
        public int StudId { get; set; }
        public string StudentName { get; set; }
        public string StudentDOB { get; set; }
        public string Gender { get; set; }
        public int ContactInfo { get; set; }
        public string Address { get; set; }
        public string AdmissionDate { get; set; }
        public int CourseEnrolled { get; set; }
        public string coursename { get; set; }
    }
    public class FacultyModels
    {
        public int Id { get; set; }
        public string FacultyName { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public int ContactInfo { get; set; }
        public string Address { get; set; }
        public int Department { get; set; }
        public string JoiningDate { get; set; }
    }



    public class AttendanceViewModel
    {
        public List<SelectListItem> StudentList { get; set; }
        public List<SelectListItem> CourseList { get; set; }
        public DateTime AttendanceDate { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }

    }
}