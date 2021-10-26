using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDiemThi.Models;
using Microsoft.EntityFrameworkCore;

namespace APIDiemThi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Score>()
                .HasKey(e => new {e.StudentId, e.SubjectId});

            modelBuilder.Entity<Score>()
                .HasOne<Student>(e => e.Student)
                .WithMany(p => p.Scores)
                .HasForeignKey(b => b.StudentId)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Score>()
                .HasOne<Subject>(e => e.Subject)
                .WithMany(p => p.Scores)
                .HasForeignKey(b => b.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                    .HasKey(s => s.StudentId);

            modelBuilder.Entity<Users>()
                        .HasOne<Student>(p => p.Student)
                        .WithOne(s => s.User)
                        .HasForeignKey<Student>(b => b.StudentId) ;

            modelBuilder.Entity<Teacher>()
                    .HasKey(s => s.TeacherId);

            modelBuilder.Entity<Users>()
                        .HasOne<Teacher>(p => p.Teacher)
                        .WithOne(s => s.User)
                        .HasForeignKey<Teacher>(b => b.TeacherId); 
        }
        

        public DbSet<Users> Users { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<Classes> Classes { get; set; }
        public DbSet<Major> Major { get; set; }
        public DbSet<Score> Score { get; set; }
        public DbSet<Subject> Subject { get; set; }
    }
}
