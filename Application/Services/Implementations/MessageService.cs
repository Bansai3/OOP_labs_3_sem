using Application.Dto;
using Application.Extensions;
using Application.Mapping;
using DataAccess;
using DataAccess.Models;

namespace Application.Services.Implementations;

public class MessageService : IMessageService
{
    private readonly DataBaseContext _dbContext;

    public MessageService(DataBaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MessageDto> CreateMessageAsync(Guid messageSourceId, string text, CancellationToken cancellationToken)
    {
        MessageSource messageSource = await _dbContext.MessageSources.GetEntityAsync(messageSourceId, cancellationToken);
        var message = new Message(Guid.NewGuid(), text, messageSource, State.New);
        messageSource.Messages.Add(message);
        _dbContext.Messages.Add(message);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return message.AsDto();
    }
}