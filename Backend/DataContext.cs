using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Backend
{
    public partial class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        public Teacher ResolveOrCreateUser(System.Security.Claims.ClaimsPrincipal user)
        {
            if (user.Identity.Name == null || user.Identity.Name.Length == 0) return null;
            var teacher = Teachers.FirstOrDefault(t => t.AuthName.Equals(user.Identity.Name));

            if (teacher == null)
            {
                Add(new Teacher { AuthName = user.Identity.Name });
                try
                {
                    SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    return null;
                }
                teacher = Teachers.FirstOrDefault(t => t.AuthName.Equals(user.Identity.Name));
            }
            return teacher;
        }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Scenario> Scenarios { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Session> Sessions { get; set; }
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
            //sessions-student
            modelBuilder.Entity<Session>()
                .HasOne<Student>(session => session.Student)
                .WithMany(student => student.Sessions);
            //sessions-scenario
            modelBuilder.Entity<Session>()
                .HasOne<Scenario>(session => session.Scenario)
                .WithMany(scenario => scenario.Sessions);
        }
    }
}
