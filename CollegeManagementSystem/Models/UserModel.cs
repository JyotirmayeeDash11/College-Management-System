using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollegeManagementSystem.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public int RoleId { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNo { get; set; }


        //Student


        public int StudId { get; set; }
        public string StudentName { get; set; }
        public string StudentDOB { get; set; }
        public string Gender { get; set; }
        public int ContactInfo { get; set; }
        public string Address { get; set; }
        public string AdmissionDate { get; set; }
        public int CourseEnrolled { get; set; }

        //Faculty

        public int FacultyId { get; set; }
        public string FacultyName { get; set; }
        public string FacultyDOB { get; set; }
        public string FacultyGender { get; set; }
        public int FacultyContactInfo { get; set; }
        public string FacultyAddress { get; set; }
        public string JoiningDate { get; set; }
    }
}