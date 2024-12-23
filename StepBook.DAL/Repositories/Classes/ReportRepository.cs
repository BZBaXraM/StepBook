namespace StepBook.DAL.Repositories.Classes;

public class ReportRepository(StepContext context) : IReportRepository
{
    public async Task<IEnumerable<Report>> GetReportsAsync()
    {
        return await context.Reports
            .Include(r => r.Reporter)
            .Include(r => r.Reported)
            .ToListAsync();
    }


    public async Task AddReportAsync(Report report)
    {
        await context.Reports.AddAsync(report);
    }
}