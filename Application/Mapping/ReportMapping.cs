using Application.Dto;
using DataAccess.Models;

namespace Application.Mapping;

public static class ReportMapping
{
    public static ReportDto AsDto(this Report report) => new ReportDto(report.Text, report.Supervisor.Id,
        report.TimePeriodInDays, report.CreationTime, report.Id);
}