using DataAccess;

namespace Application.Services.Implementations;

public class SameUsersCheckService : ISameUsersCheckService
{
    private readonly DataBaseContext _dbContext;

    public SameUsersCheckService(DataBaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool CheckSameUsers(string login) => _dbContext.Accounts.Any(ac => ac.Login == login);
}