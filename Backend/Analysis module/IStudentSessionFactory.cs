using Backend.Models;

namespace Backend.Analysis_module
{
    public interface IStudentSessionFactory
    {
        StudentSessionModule Create(string studentEmail, int studentId, string code, string sessionId);

        StudentSessionModule Create(string requestEmail, int testLimit, string requestCode, string id, Session userSession);
    }
}