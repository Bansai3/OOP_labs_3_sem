using Application.Dto;
using Application.Exceptions;
using Application.Extensions;
using Application.Mapping;
using DataAccess;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using AuthenticationException = System.Security.Authentication.AuthenticationException;

namespace Application.Services.Implementations;

public class AuthenticationService : IAuthenticationService
{

    private readonly DataBaseContext _dbContext;

    public AuthenticationService(DataBaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<AccountDto> CheckAuthenticationAsync(string login, string password, CancellationToken cancellationToken)
    {
        Account? account =
            await _dbContext.Accounts.SingleOrDefaultAsync(x => x.Login == login && x.Password == password, cancellationToken: cancellationToken);
        if (account is null)
            throw new AccountNotFoundException($"Account with login {login} and password {password} does not exist!");
        Guid employeeId = account.Employee.Id;
        Employee employee = await _dbContext.Employees.GetEntityAsync(employeeId, cancellationToken);
        employee.HaveAccess = true;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return account.AsDto();
    }
}