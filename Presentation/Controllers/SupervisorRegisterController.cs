using Application.Dto;
using Application.Exceptions;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;
[ApiController]
[Route("api/Create supervisor")]
public class SupervisorRegisterController : ControllerBase
{
    private readonly ISupervisorRegisterService _service;
    private readonly IAccountService _account;
    private readonly ISameUsersCheckService _check;

    public SupervisorRegisterController(ISupervisorRegisterService service, IAccountService account, ISameUsersCheckService check)
    {
        _service = service;
        _account = account;
        _check = check;
    }
    
    public CancellationToken CancellationToken => HttpContext.RequestAborted;
    
    [HttpPost]
    public async Task<ActionResult<SupervisorDto>> CreateSupervisorAsync([FromBody] CreateSupervisor model)
    {
        if (_check.CheckSameUsers(model.Login))
            throw new SameUsersException($"User with login {model.Login} already exists!");
        Task<SupervisorDto> supervisorDto = _service.CreateSupervisorAsync(model.Name, CancellationToken);
        await _account.CreateAccountAsync(model.Login, model.Password, supervisorDto.Result.Id, CancellationToken);
        return Ok(supervisorDto);
    }
}