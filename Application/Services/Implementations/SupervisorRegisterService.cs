using Application.Dto;
using Application.Mapping;
using DataAccess;
using DataAccess.Models;

namespace Application.Services.Implementations;

public class SupervisorRegisterService : ISupervisorRegisterService
{
    private readonly DataBaseContext _dbContext;

    public SupervisorRegisterService(DataBaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SupervisorDto> CreateSupervisorAsync(string name, CancellationToken cancellationToken)
    {
        var supervisor = new Supervisor(name, Guid.NewGuid());
        _dbContext.Employees.Add(supervisor);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return supervisor.AsDto();
    }
    
}