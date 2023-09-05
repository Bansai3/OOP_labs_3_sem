using Application.Dto;
using DataAccess.Models;

namespace Application.Mapping;

public static class SupervisorMapping
{
    public static SupervisorDto AsDto(this Supervisor supervisor) =>
        new SupervisorDto(supervisor.Subordinates.Select(x => x.AsDto()).ToArray(), supervisor.Id, supervisor.Name);
}