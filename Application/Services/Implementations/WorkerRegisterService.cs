using Application.Dto;
using Application.Exceptions;
using Application.Extensions;
using Application.Mapping;
using DataAccess;
using DataAccess.Models;

namespace Application.Services.Implementations;

public class WorkerRegisterService : IWorkerRegisterService
{
    private readonly DataBaseContext _dbContext;

    public WorkerRegisterService(DataBaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<WorkerDto> CreateWorkerAsync(string name, Guid supervisorId, CancellationToken cancellationToken)
    {
        Employee employee = await _dbContext.Employees.GetEntityAsync(supervisorId, cancellationToken);
        if (employee is not Supervisor supervisor)
            throw new EmployeeTypeException("Employee must be supervisor!");
        var worker = new Worker(name, Guid.NewGuid(), supervisor);
        _dbContext.Employees.Add(worker);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return worker.AsDto();
    }
}