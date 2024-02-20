using CollegeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CollegeManagementSystem.Models
{

        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext() : base("DbContext")
            {
            }

            public DbSet<Users> Users { get; set; }
            public DbSet<UserRole> UserRole { get; set; }
            public DbSet<UserDepartment> UserDepartment { get; set; }
            public DbSet<Roles> Roles { get; set; }
            public DbSet<Students> Students { get; set; }
            public DbSet<Results> Results { get; set; }
            public DbSet<LibraryTransactions> LibraryTransactions { get; set; }
            public DbSet<LibraryBooks> LibraryBooks { get; set; }
            public DbSet<Fees> Fees { get; set; }
            public DbSet<Faculty> Faculty { get; set; }
            public DbSet<ExamNotice> ExamNotice { get; set; }
            public DbSet<Departments> Departments { get; set; }
            public DbSet<Courses> Courses { get; set; }
            public DbSet<Attendance> Attendance { get; set; }

        public System.Data.Entity.DbSet<CollegeManagementSystem.Models.FacultyModels> FacultyModels { get; set; }
    }
    
}