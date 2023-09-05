using Application.Dto;

namespace Application.Services;

public interface IHandleMessagesService
{
    Task<MessageHandlerDto> HandleMessagesAsync(Guid workerId, CancellationToken cancellationToken);
}