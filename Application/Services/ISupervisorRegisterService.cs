using Application.Dto;
using DataAccess.Models;

namespace Application.Services;

public interface ISupervisorRegisterService
{
    Task<SupervisorDto> CreateSupervisorAsync(string name, CancellationToken cancellationToken);
}