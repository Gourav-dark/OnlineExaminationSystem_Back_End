﻿using Microsoft.EntityFrameworkCore;
using OnlineExaminationSystem_Back_End_DAL.Models.DBModels;
using System;
using System.Security.Policy;

namespace OnlineExaminationSystem_Back_End_DAL.Data
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options){}
        public virtual DbSet<InstituteDetail> InstituteDetails { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<EnrollStudent> EnrollStudents { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<ExamDetail> ExamDetails { get; set; }
        public virtual DbSet<Result> Results { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //---------Institute Details Table----------
            //One Institute hav multiple users
            //modelBuilder.Entity<InstituteDetail>()
            //    .HasMany(i => i.Users)
            //    .WithOne(u => u.InstituteDetail)
            //    .HasForeignKey(u => u.InstituteId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //////////achive same using user and null value
            modelBuilder.Entity<User>()
                .HasOne(u => u.InstituteDetail)
                .WithMany(i => i.Users)
                .HasForeignKey(u => u.InstituteId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
            // One Institute hav multiple course
            modelBuilder.Entity<InstituteDetail>()
                .HasMany(i => i.Courses)
                .WithOne(c => c.InstituteDetail)
                .HasForeignKey(c => c.InstituteId)
                .OnDelete(DeleteBehavior.Cascade);

            //----------Role Table
            //Role have multiple uses
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            //-------User Table
            //if user is student than one Course Enrolled---------->
            modelBuilder.Entity<User>()
                .HasOne(u => u.EnrollStudent)
                .WithOne(s => s.Student)
                .HasForeignKey<EnrollStudent>(u=>u.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            //if user is examiner than create many Question
            modelBuilder.Entity<User>()
                .HasMany(u => u.Questions)
                .WithOne(e => e.Examiner)
                .HasForeignKey(e => e.ExaminerId)
                .OnDelete(DeleteBehavior.ClientCascade);
            //if user is student than have many result
            modelBuilder.Entity<User>()
                .HasMany(u => u.Results)
                .WithOne(s => s.Student)
                .HasForeignKey(s => s.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
            //------------------------------------------------------->
            //----------Course table
            //one course have multiple subject----->
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Subjects)
                .WithOne(s => s.Course)
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
            //one course have multiple enroll student
            modelBuilder.Entity<Course>()
                .HasMany(c => c.EnrollStudents)
                .WithOne(s => s.Course)
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.ClientCascade);

            //-----------Subject Table
            //one subject have many question
            modelBuilder.Entity<Subject>()
                .HasMany(s => s.Questions)
                .WithOne(q => q.Subject)
                .HasForeignKey(q => q.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);
            //one Subject have many Exam
            modelBuilder.Entity<Subject>()
                .HasMany(s => s.ExamDetails)
                .WithOne(q => q.Subject)
                .HasForeignKey(q => q.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            //-----Exam Details----Exam Details have multiple Result
            modelBuilder.Entity<ExamDetail>()
                .HasMany(s => s.Results)
                .WithOne(q => q.ExamDetail)
                .HasForeignKey(q => q.ExamId)
                .OnDelete(DeleteBehavior.ClientCascade);

        }
    }
}
