using Application.Dto;

namespace Application.Services;

public interface IAddMessageSourceToWorkerService
{
    Task<MessageSourceConnectionDto> AddMessageSourceToWorkerAsync(Guid workerId, Guid messageSourceId, CancellationToken cancellationToken);
}