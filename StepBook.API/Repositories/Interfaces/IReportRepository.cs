namespace StepBook.API.Repositories.Interfaces;

public interface IReportRepository
{
    Task<IEnumerable<Report>> GetReportsAsync();
    Task AddReportAsync(Report report);
}