namespace Backend.Analysis_module.SessionModule
{
    public interface ISessionFactory
    {
        SessionModuleService Create(string studentEmail, int studentId, string code, string sessionId);

        SessionModuleService Create(string requestEmail, int testLimit, string requestCode, string id, Backend.Models.Session userSession);
    }
}