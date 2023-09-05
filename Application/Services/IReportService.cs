using Application.Dto;

namespace Application.Services;

public interface IReportService
{
    Task<ReportDto> CreateReportAsync(Guid supervisorId, int timePeriodInDays, CancellationToken cancellationToken);
}