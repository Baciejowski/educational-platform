using Backend.Models;

namespace Backend.Analysis_module.SessionModule
{
    public class SessionFactory : ISessionFactory
    {
        public SessionModuleService Create(string studentEmail, int studentId, string code, string sessionId)
        {
            return new SessionModuleService(studentEmail, studentId, code, sessionId);
        }

        public SessionModuleService Create(string requestEmail, int testLimit, string requestCode, string sessionId,
            Session userSession)
        {
            return new SessionModuleService(requestEmail, testLimit, requestCode, sessionId, userSession);
        }
    }
}