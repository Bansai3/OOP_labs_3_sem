using Application.Dto;
using Application.Exceptions;
using Application.Extensions;
using Application.Mapping;
using DataAccess;
using DataAccess.Models;

namespace Application.Services.Implementations;

public class MessageLoadingService : IMessageLoadingService
{
    private readonly DataBaseContext _dbContext;

    public MessageLoadingService(DataBaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MessageLoadingDto> LoadMessagesFromMessageSourceAsync(Guid messageSourceId, CancellationToken cancellationToken)
    {
        MessageSource messageSource = await _dbContext.MessageSources.GetEntityAsync(messageSourceId, cancellationToken);
        AddMessagesToDbContext(messageSource.Messages);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new MessageLoadingDto(messageSourceId, messageSource.Messages.Select(x => x.AsDto()).ToArray());
    }
    private void AddMessagesToDbContext(ICollection<Message> messages)
    {
        foreach (Message message in messages)
        {
            if (message.State == State.New)
            {
                message.State = State.Received;
            }
        }
    }
}