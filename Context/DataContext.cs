using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using trivia_backend.Models;

namespace trivia_backend.Context
{
    public class DataContext : DbContext
    {
        
        public DataContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<UserModel> Users { get; set;}
        public DbSet<QuizModel> Quizzes { get; set;}
        public DbSet<UserModel> Questions { get; set;}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // One Quiz has many Questions
        modelBuilder.Entity<QuestionModel>()
            .HasOne(q => q.Quiz)               // Navigation property
            .WithMany(z => z.Questions)        // Collection in Quiz
            .HasForeignKey(q => q.QuizId)      // Foreign key in Question
            .OnDelete(DeleteBehavior.Cascade); // Optional: deletes questions if quiz is deleted
    }

    }
}