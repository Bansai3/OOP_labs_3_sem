using Application.Dto;
using Application.Exceptions;
using Application.Extensions;
using Application.Mapping;
using DataAccess;
using DataAccess.Models;

namespace Application.Services.Implementations;

public class HandleMessagesService : IHandleMessagesService
{
    private readonly DataBaseContext _dbContext;

    public HandleMessagesService(DataBaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<MessageHandlerDto> HandleMessagesAsync(Guid workerId, CancellationToken cancellationToken)
    {
        Employee employee = await _dbContext.Employees.GetEntityAsync(workerId, cancellationToken);
        if (employee is not Worker worker)
            throw new EmployeeTypeException("Employee must be worker!");
        if (worker.HaveAccess == false)
            throw new EmployeeAccessException("Worker does not have access!");
        ICollection<Message> messages = GetWorkerReceivedMessages(worker);
        HandleMessages(messages);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new MessageHandlerDto(workerId, messages.Select(x => x.AsDto()).ToArray());
    }

    private ICollection<Message> GetWorkerReceivedMessages(Worker worker)
    {
        var messages = new List<Message>();
        ICollection<MessageSource> messageSources = worker.MessageSources;
        foreach (MessageSource messageSource in messageSources)
        {
           messages.AddRange(messageSource.Messages.Where(x => x.State == State.Received).ToList());
        }

        return messages;
    }

    private void HandleMessages(ICollection<Message> messages)
    {
        foreach (Message message in messages)
        {
            message.State = State.Processed;
        }
    }
}