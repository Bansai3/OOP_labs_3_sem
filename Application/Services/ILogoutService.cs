namespace Application.Services;

public interface ILogoutService
{
    void Logout(string login, CancellationToken cancellationToken);
}