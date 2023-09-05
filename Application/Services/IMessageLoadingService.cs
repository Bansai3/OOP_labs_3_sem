using Application.Dto;

namespace Application.Services;

public interface IMessageLoadingService
{
    Task<MessageLoadingDto> LoadMessagesFromMessageSourceAsync(Guid messageSourceId, CancellationToken cancellationToken);
}