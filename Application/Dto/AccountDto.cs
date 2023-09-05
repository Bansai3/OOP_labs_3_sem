namespace Application.Dto;

public record class AccountDto(string Login, string Password, Guid EmployeeId, Guid Id);