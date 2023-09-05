using Application.Dto;
using Application.Exceptions;
using Application.Extensions;
using Application.Mapping;
using DataAccess;
using DataAccess.Models;

namespace Application.Services.Implementations;

public class AccountService : IAccountService
{
    private readonly DataBaseContext _dbContext;

    public AccountService(DataBaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<AccountDto> CreateAccountAsync(string login, string password, Guid employeeId, CancellationToken cancellationToken)
    {
        Employee employee = await _dbContext.Employees.GetEntityAsync(employeeId, cancellationToken);
        var account = new Account(login, password, Guid.NewGuid(), employee);
        _dbContext.Accounts.Add(account);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return account.AsDto();
    }
}