using Backend.Models;

namespace Backend.Analysis_module
{
    public interface IStudentSessionFactory
    {
        IStudentSessionModule Create(string studentEmail, int studentId, string code, string sessionId);

        IStudentSessionModule Create(string requestEmail, int testLimit, string requestCode, string id, Session userSession);
    }
}