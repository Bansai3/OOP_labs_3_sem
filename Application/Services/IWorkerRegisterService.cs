using Application.Dto;

namespace Application.Services;

public interface IWorkerRegisterService
{
    Task<WorkerDto> CreateWorkerAsync(string name, Guid supervisorId, CancellationToken cancellationToken);
}