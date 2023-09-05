using Application.Dto;
using Application.Exceptions;
using Application.Mapping;
using DataAccess;
using DataAccess.Models;

namespace Application.Services.Implementations;

public class MessageSourceService : IMessageSourceService
{
    private readonly DataBaseContext _dbContext;

    public MessageSourceService(DataBaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MessageSourceDto> CreateMessageSourceAsync(string messageSourceType, CancellationToken cancellationToken)
    {
        MessageSource messageSource = DefineMessageSourceType(messageSourceType);
        _dbContext.MessageSources.Add(messageSource);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return messageSource.AsDto();
    }

    private MessageSource DefineMessageSourceType(string messageSourceType)
    {
        return messageSourceType switch
        {
            "Email" => new Email(Guid.NewGuid()),
            "Messenger" => new Messenger(Guid.NewGuid()),
            "Phone" => new Phone(Guid.NewGuid()),
            _ => throw new InvalidMessageSourceTypeException($"Invalid message source type: {messageSourceType}!")
        };
    }
}