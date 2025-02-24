using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NL_THUD.Models;
using System.Reflection.Emit;

namespace NL_THUD.Data
{
    public class ApplicationDbContext : IdentityDbContext<Person>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

            // Cấu hình quan hệ One-to-One giữa Students và Parents
            modelBuilder.Entity<Students>()
                .HasOne(s => s.Parents)
                .WithOne(p => p.Students)
                .HasForeignKey<Students>(s => s.ParentId)
                .OnDelete(DeleteBehavior.Restrict);  // Cấu hình xóa theo cách mong muốn

            //Cấu hình quan hệ với bảng phujd Teacher_Class
            modelBuilder.Entity<Teacher_Class>()
                .HasKey(tc => new { tc.Teacher_Id, tc.Class_Id });
            modelBuilder.Entity<Teacher_Class>()
                .HasOne(tc => tc.Teacher)
                .WithMany(t => t.TeacherClass)
                .HasForeignKey(tc => tc.Teacher_Id);
            modelBuilder.Entity<Teacher_Class>()
                .HasOne(tc => tc.Class)
                .WithMany(c => c.TeacherClass)
                .HasForeignKey(tc => tc.Class_Id);

            // Cấu hình quan hệ với bảng phụ Class_Student
            modelBuilder.Entity<Class_Student>()
                .HasKey(cs => new {cs.Student_Id, cs.Class_Id});
            modelBuilder.Entity<Class_Student>()
                .HasOne(cs => cs.Students)
                .WithMany(s => s.Class_Students)
                .HasForeignKey(cs => cs.Student_Id);
            modelBuilder.Entity<Class_Student>()
                .HasOne(cs => cs.Class)
                .WithMany(c => c.Class_Student)
                .HasForeignKey(cs => cs.Class_Id);
            modelBuilder.Entity<Class_Student>()
                .HasOne(cs => cs.schedules)
                .WithMany(s => s.Class_Students)
                .HasForeignKey(cs => cs.Schedule_Id);

            //Cấu hình quan hệ với bảng phục Teacher_Subject
            modelBuilder.Entity<Teacher_Subject>()
                .HasKey(ts => new { ts.Subject_Id, ts.Teacher_Id, ts.Class_Id });
            modelBuilder.Entity<Teacher_Subject>()
                .HasOne(ts => ts.Subjects)
                .WithMany(s => s.Teacher_Subjects)
                .HasForeignKey(ts => ts.Subject_Id);
            modelBuilder.Entity<Teacher_Subject>()
                .HasOne(ts => ts.Teacher)
                .WithMany(t => t.TeacherSubject)
                .HasForeignKey(ts => ts.Teacher_Id);
            modelBuilder.Entity<Teacher_Subject>()
                .HasOne(ts => ts.Classes)
                .WithMany(c => c.TeacherSubject)
                .HasForeignKey(ts => ts.Class_Id);

            //Cấu hình quan hệ với bảng phụ Schedule_Detail
            modelBuilder.Entity<Schedule_Detail>()
                .HasKey(sd => new { sd.SubjectId, sd.ScheduleId });
            modelBuilder.Entity<Schedule_Detail>()
                .HasOne(sd => sd.Subjects)
                .WithMany(s => s.Schedule_Detail)
                .HasForeignKey(sd => sd.SubjectId);
            modelBuilder.Entity<Schedule_Detail>()
                .HasOne(sd => sd.Schedules)
                .WithMany(s => s.Schedules_Detail)
                .HasForeignKey(sd => sd.ScheduleId);
           
            // Cấu hình quan hệ với bảng phụ Student_Score
            modelBuilder.Entity<Student_Score>()
                .HasKey(ss => new {ss.StudentId, ss.ScoreId, ss.SubjectId, ss.ATId});
            modelBuilder.Entity<Student_Score>()
                .HasOne(ss => ss.Students)
                .WithMany(s => s.student_Scores)
                .HasForeignKey(ss => ss.StudentId);
            modelBuilder.Entity<Student_Score>()
                .HasOne(ss => ss.Score)
                .WithMany(s => s.student_Scores)
                .HasForeignKey(ss => ss.ScoreId);
            modelBuilder.Entity<Student_Score>()
                .HasOne(ss => ss.Subjects)
                .WithMany(s => s.student_Scores)
                .HasForeignKey(ss => ss.SubjectId);
            modelBuilder.Entity<Student_Score>()
                .HasOne(ss => ss.AcademicTranscript)
                .WithMany(s => s.StudentScores)
                .HasForeignKey(ss => ss.ATId);
        }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<Parents> Parents { get; set; }
        public DbSet<SystemAdmin> SystemAdmins { get; set; }
        public DbSet<Address> Addressses { get; set; }
        public DbSet<Provinces> Provinces { get; set; }
        public DbSet<Districts> Districts { get; set; }
        public DbSet<Wards> Wards { get; set; }
        public DbSet<AcademicTranscript> AcademicTranscripts { get; set; }
        public DbSet<Score> Scores { get; set; }    
        public DbSet<Schedules> Schedules { get; set; }
        public DbSet<ManagementStaff> ManagementStaffs { get; set; }
    }
}
