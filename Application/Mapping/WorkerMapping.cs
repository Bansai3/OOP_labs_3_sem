using Application.Dto;
using DataAccess.Models;

namespace Application.Mapping;

public static class WorkerMapping
{
    public static WorkerDto AsDto(this Worker worker) =>
        new WorkerDto(worker.MessageSources.Select(x => x.AsDto()).ToArray(), worker.Supervisor.Id, worker.Id, worker.Name);
}