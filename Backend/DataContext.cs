using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend
{
    public partial class DataContext : DbContext
    {
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Scenario> Scenarios { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //answers-question
            modelBuilder.Entity<Answer>()
                 .HasOne<Question>(a => a.Question)
                 .WithMany(q => q.ABCDAnswers);
            //students-class
            modelBuilder.Entity<Student>()
                .HasOne<Class>(s => s.Class)
                .WithMany(c => c.Students);
            //classes-teacher
            modelBuilder.Entity<Class>()
                .HasOne<Teacher>(c => c.Teacher)
                .WithMany(t => t.Classes);
            //topics-teacher
            modelBuilder.Entity<Topic>()
                .HasOne<Teacher>(topic => topic.Teacher)
                .WithMany(teacher => teacher.Topics);
            //scenarios-topic
            modelBuilder.Entity<Scenario>()
                .HasOne<Topic>(s => s.Topic)
                .WithMany(t => t.Scenarios);
            //games-student
            modelBuilder.Entity<Game>()
                .HasOne<Student>(g => g.Student)
                .WithMany(s => s.Games);
            //games-scenario
            modelBuilder.Entity<Game>()
                .HasOne<Scenario>(g => g.Scenario)
                .WithMany(s => s.Games);
            //scenarios-questions
            modelBuilder.Entity<Scenario>()
                .HasMany<Question>(s => s.Questions)
                .WithMany(q => q.Scenarios);
        }
    }
}
