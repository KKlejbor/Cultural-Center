using System;
using Cultural_Center.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Cultural_Center
{
    public partial class CulturalCenterContext : DbContext
    {
        public CulturalCenterContext()
        {
        }

        public CulturalCenterContext(DbContextOptions<CulturalCenterContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Cultural_Center;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Polish_CI_AS");
            modelBuilder.Entity<Subjects>()
                .HasOne<Instructors>(s => s.Instructor)
                .WithMany(i => i.Subjects)
                .HasForeignKey(s => s.InstructorsId);
            modelBuilder.Entity<Lessons>()
                .HasOne<Subjects>(l => l.Subject)
                .WithMany(s => s.Lessons)
                .HasForeignKey(l => l.SubjectsId);
            modelBuilder.Entity<StudentGroups>()
                .HasOne<Lessons>(sg => sg.Lesson)
                .WithMany(l => l.StudentGroups)
                .HasForeignKey(sg => sg.LessonsId);
            modelBuilder.Entity<StudentGroupsStudents>()
                .HasKey(sgs => new { sgs.StudentGroupsId, sgs.StudentsId });
            modelBuilder.Entity<StudentGroupsStudents>()
                .HasOne<StudentGroups>(sgs => sgs.StudentGroup)
                .WithMany(sg => sg.StudentGroupsStudents)
                .HasForeignKey(sgs => sgs.StudentGroupsId);
            modelBuilder.Entity<StudentGroupsStudents>()
                .HasOne<StudentGroups>(sgs => sgs.StudentGroup)
                .WithMany(sg => sg.StudentGroupsStudents)
                .HasForeignKey(sgs => sgs.StudentGroupsId);
            modelBuilder.Entity<StudentGroupsStudents>()
                .HasOne<Students>(sgs => sgs.Student)
                .WithMany(s => s.StudentGroupsStudents)
                .HasForeignKey(sgs => sgs.StudentsId);

            OnModelCreatingPartial(modelBuilder);
        }
        
        public DbSet<Instructors> Instructors { get; set; }
        public DbSet<Subjects> Subjects { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public DbSet<Cultural_Center.Models.Students> Students { get; set; }

        public DbSet<Cultural_Center.Models.Lessons> Lessons { get; set; }

        public DbSet<Cultural_Center.Models.StudentGroups> StudentGroups { get; set; }
    }
}
