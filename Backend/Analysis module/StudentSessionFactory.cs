using Backend.Models;

namespace Backend.Analysis_module
{
    public class StudentSessionFactory: IStudentSessionFactory
    {
        protected readonly DataContext Context;

        public StudentSessionFactory(DataContext context)
        {
            Context = context;
        }

        public IStudentSessionModule Create(string studentEmail, int studentId, string code, string sessionId)
        {
            return new StudentSessionModule( studentEmail,  studentId,  code,  sessionId, Context);
        }
        public IStudentSessionModule Create(string requestEmail, int testLimit, string requestCode, string sessionId,
            Session userSession)
        {
            return new StudentSessionModule(requestEmail, testLimit, requestCode, sessionId, userSession, Context);
        }
    }
}