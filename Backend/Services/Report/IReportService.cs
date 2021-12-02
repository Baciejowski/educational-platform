namespace Backend.Services.Report
{
    public interface IReportService
    {
        public string TimePerSkillsGraph();
        public string TimePerAttemptGraph();
        public GeneralReportDto GetAllGeneralGraphs();
    }
}