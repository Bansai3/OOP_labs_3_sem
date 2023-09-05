using Application.Exceptions;
using Application.Extensions;
using DataAccess;
using DataAccess.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations;

public class LogoutService : ILogoutService
{
    private readonly DataBaseContext _dbContext;

    public LogoutService(DataBaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Logout(string login, CancellationToken cancellationToken)
    {
        Account? account = _dbContext.Accounts.SingleOrDefault(ac => ac.Login == login);
        if (account is null)
            throw new AccountNotFoundException($"Account with login {login} does not exist!");
        account.Employee.HaveAccess = false;
        _dbContext.SaveChangesAsync(cancellationToken);
    }
}