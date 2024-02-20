using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CollegeManagementSystem.Models
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
    }



    [Table("UserRole")]
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

    }

    [Table("UserDepartment")]
    public class UserDepartment
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DeptId { get; set; }

    }

    [Table("Roles")]
    public class Roles
    {
        [Key]
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string DisplayName { get; set; }

    }






    [Table("Students")]
    public class Students
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }

        public string StudentName { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public int ContactInfo { get; set; }
        public string Address { get; set; }
        public string AdmissionDate { get; set; }
        public int CourseEnrolled { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
    }



    [Table("Results")]
    public class Results
    {
        [Key]
        public int Id { get; set; }
        public int StudentID { get; set; }
        public int CourseID { get; set; }
        public int MarksObtained { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
    }






    [Table("LibraryTransactions")]
    public class LibraryTransactions
    {
        [Key]
        public int Id { get; set; }
        public int StudentID { get; set; }
        public int BookID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
    }






    [Table("LibraryBooks")]
    public class LibraryBooks
    {
        [Key]
        public int Id { get; set; }
        public string BookTitle { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int PublicationYear { get; set; }
        public string AvailabilityStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
    }




    [Table("Fees")]
    public class Fees
    {
        [Key]
        public int Id { get; set; }
        public int StudentID { get; set; }
        public int CourseID { get; set; }
        public int DeptId { get; set; }
        public decimal CourseFee { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
    }



    [Table("Faculty")]
    public class Faculty
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FacultyName { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public int ContactInfo { get; set; }
        public string Address { get; set; }
        public int Department { get; set; }
        public string JoiningDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
    }



    [Table("ExamNotice")]
    public class ExamNotice
    {
        [Key]
        public int Id { get; set; }
        public int CourseID { get; set; }
        public string ExamDate { get; set; }
        public string ExamName { get; set; }
        
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
    }

    [Table("Departments")]
    public class Departments
    {
        [Key]
        public int Id { get; set; }
        public string DeptName { get; set; }
        public string DeptCode { get; set; }
        public string DeptShortName { get; set; }
       
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
    }

    [Table("Courses")]
    public class Courses
    {
        [Key]
        public int Id { get; set; }
        public string CourseName { get; set; }
        public int DeptId { get; set; }
        public string CourseDescription { get; set; }
        public int CourseDuration { get; set; }
        public decimal CourseFee { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
    }

    [Table("Attendance")]
    public class Attendance
    {
        [Key]
        public int Id { get; set; }
        public int StudentID { get; set; }
        public int CourseID { get; set; }
        public DateTime AttendanceDate { get; set; }
        public bool Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
    }


}