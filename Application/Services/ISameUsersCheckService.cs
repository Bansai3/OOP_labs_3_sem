namespace Application.Services;

public interface ISameUsersCheckService
{
    bool CheckSameUsers(string login);
}