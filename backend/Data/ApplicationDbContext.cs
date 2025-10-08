using Microsoft.EntityFrameworkCore;
using StudentScoreTracker.API.Models;

namespace StudentScoreTracker.API.Data
{ 
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

            public DbSet<User> Users { get; set; }
            public DbSet<Student> Students { get; set; }
            public DbSet<Grade> Grades { get; set; }
            public DbSet<Attendance> Attendance { get; set; }
            public DbSet<Feedback> Feedbacks { get; set; }
  
    }

    }
