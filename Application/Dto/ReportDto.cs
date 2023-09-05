namespace Application.Dto;

public record class ReportDto(string Report, Guid SupervisorId, int TimePeriodInDays, DateTime CreationTime, Guid Id);