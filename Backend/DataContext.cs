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

        public DbSet<AnalysisResult> AnalysisResults { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<AnsweredQuestion> AnsweredQuestions { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameplayData> GameplayData { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Scenario> Scenarios { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionRecord> SessionRecords { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Topic> Topics { get; set; }

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
            //sessions-sessionRecords
            modelBuilder.Entity<SessionRecord>()
                .HasOne<Session>(sessionRecords => sessionRecords.Session)
                .WithOne(session => session.SessionRecord)
                .HasForeignKey<SessionRecord>(p => p.SessionRecordID);
            //sessions-analysisResult
            modelBuilder.Entity<SessionRecord>()
                .HasOne<AnalysisResult>(sessionRecords => sessionRecords.AnalysisResult)
                .WithOne(analysisResult => analysisResult.SessionRecord)
                .HasForeignKey<SessionRecord>(p => p.SessionRecordID);
            //sessions-gameplayData
            modelBuilder.Entity<SessionRecord>()
                .HasOne<GameplayData>(sessionRecords => sessionRecords.GameplayData)
                .WithOne(gameplayData => gameplayData.SessionRecord)
                .HasForeignKey<SessionRecord>(p => p.SessionRecordID);
            //analysisResult-answeredQuestions
            modelBuilder.Entity<AnsweredQuestion>()
                .HasOne<AnalysisResult>(answeredQuestions => answeredQuestions.AnalysisResult)
                .WithMany(analysisResult => analysisResult.AnsweredQuestions);
            //answeredQuestion-answers
            modelBuilder.Entity<AnsweredQuestion>()
                .HasMany<Answer>(answeredQuestion => answeredQuestion.AnsweredAnswers)
                .WithMany(answers => answers.AnsweredQuestions);
        }
    }
}