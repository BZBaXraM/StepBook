namespace StepBook.API.Repositories.Classes;

public class ReportRepository : IReportRepository
{
    private readonly StepContext context;

    public ReportRepository(StepContext context)
    {
        this.context = context;
    }

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