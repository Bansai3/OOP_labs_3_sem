using Application.Dto;
using DataAccess.Models;

namespace Application.Services;

public interface IAuthenticationService
{
    Task<AccountDto> CheckAuthenticationAsync(string login, string password, CancellationToken cancellationToken);
}