using Application.Dto;
using Application.Exceptions;
using Application.Extensions;
using Application.Mapping;
using DataAccess;
using DataAccess.Models;

namespace Application.Services.Implementations;

public class ReportService : IReportService
{
    private readonly DataBaseContext _dbContext;

    public ReportService(DataBaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ReportDto> CreateReportAsync(Guid supervisorId, int timePeriodInDays, CancellationToken cancellationToken)
    {
        Employee employee = await _dbContext.Employees.GetEntityAsync(supervisorId, cancellationToken);
        if (employee is not Supervisor supervisor)
            throw new EmployeeTypeException("Employee must be supervisor!");
        var days = new TimeSpan(timePeriodInDays, 0, 0, 0);
        DateTime timeNow = DateTime.Now;
        DateTime periodStartTime = timeNow.Subtract(days);
        int processedMessagesAmount = GetProcessedMessagesAmount(supervisor);
        Dictionary<MessageSource, int> messageSourcesMessagesAmount = GetMessageSourcesMessagesAmount(supervisor);
        int messagesAmountDuringPeriodOfTime =
            GetMessagesAmountDuringPeriodOfTime(periodStartTime, timeNow, supervisor);
        string reportText = CreateReportInformation(processedMessagesAmount, messageSourcesMessagesAmount,
            messagesAmountDuringPeriodOfTime, periodStartTime, timeNow);
        var report = new Report(Guid.NewGuid(), reportText, supervisor, timePeriodInDays, timeNow);
        _dbContext.Reports.Add(report);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return report.AsDto();
    }

    private int GetProcessedMessagesAmount(Supervisor supervisor)
    {
        ICollection<Worker> workers = supervisor.Subordinates;
        return workers.Sum(worker => worker.MessageSources.Sum(messageSource => messageSource.Messages.Count(x => x.State == State.Processed)));
    }

    private Dictionary<MessageSource, int> GetMessageSourcesMessagesAmount(Supervisor supervisor)
    {
        var messageSourceInf = new Dictionary<MessageSource, int>();
        ICollection<Worker> workers = supervisor.Subordinates;
        foreach (Worker worker in workers)
        {
            foreach (MessageSource messageSource in worker.MessageSources)
            {
                if (messageSourceInf.ContainsKey(messageSource) == false)
                {
                    messageSourceInf.Add(messageSource, messageSource.Messages.Count);
                }
            }
        }

        return messageSourceInf;
    }

    private int GetMessagesAmountDuringPeriodOfTime(DateTime startDate, DateTime endDate, Supervisor supervisor)
    {
        return supervisor.Subordinates.Sum(w =>
            w.MessageSources.Sum(ms => ms.Messages.Count(m => m.Date >= startDate && m.Date <= endDate)));
    }

    private string CreateReportInformation
        (int processedMessagesAmount, 
        Dictionary<MessageSource, int> messageSourcesMessagesAmount, 
        int messagesAmountDuringPeriodOfTime,
        DateTime startTime,
        DateTime endTime)
    {
        string information =
            $"Report:\n" +
            $"Processed messages amount: {processedMessagesAmount}\n" +
            $"Messages amount during period {startTime.Date} - {endTime.Date}: {messagesAmountDuringPeriodOfTime}\n" +
            "Message sources:\n";
        int messageSourceCount = 1;
        foreach (KeyValuePair<MessageSource, int> messageSource in messageSourcesMessagesAmount)
        {
            information +=
                $"{messageSourceCount++}) source: {messageSource.Key.Title}; id: {messageSource.Key.Id}; messages amount: {messageSource.Value}";
        }

        return information;
    }
}