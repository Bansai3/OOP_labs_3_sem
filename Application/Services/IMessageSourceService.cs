using Application.Dto;

namespace Application.Services;

public interface IMessageSourceService
{
    Task<MessageSourceDto> CreateMessageSourceAsync(string messageSourceType, CancellationToken cancellationToken);
}