using Application.Dto;

namespace Application.Services;

public interface IAccountService
{
    Task<AccountDto> CreateAccountAsync(string login, string password, Guid employeeId, CancellationToken cancellationToken);
}