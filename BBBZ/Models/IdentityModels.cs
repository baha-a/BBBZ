using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace BBBZ.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Semester> Semester { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments{ get; set; }
        public DbSet<Message> Messages{ get; set; }
        public DbSet<Class> Classes{ get; set; }
        public DbSet<Attendance> Attendances{ get; set; }
        public DbSet<Lecture> Lectures{ get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }
}