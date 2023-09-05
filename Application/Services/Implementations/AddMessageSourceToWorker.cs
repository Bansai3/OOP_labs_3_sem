using Application.Dto;
using Application.Exceptions;
using Application.Extensions;
using DataAccess;
using DataAccess.Models;

namespace Application.Services.Implementations;

public class AddMessageSourceToWorkerService : IAddMessageSourceToWorkerService
{
    private readonly DataBaseContext _dbContext;

    public AddMessageSourceToWorkerService(DataBaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<MessageSourceConnectionDto> AddMessageSourceToWorkerAsync(Guid workerId, Guid messageSourceId, CancellationToken cancellationToken)
    {
        Employee employee = await _dbContext.Employees.GetEntityAsync(workerId, cancellationToken);
        MessageSource messageSource = await _dbContext.MessageSources.GetEntityAsync(messageSourceId, cancellationToken);
        if (employee is not Worker worker)
            throw new EmployeeTypeException("Employee must be worker!");
        worker.MessageSources.Add(messageSource);
        messageSource.Workers.Add(worker);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new MessageSourceConnectionDto(workerId, messageSourceId);
    }
}