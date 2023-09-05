using Application.Dto;
using Application.Exceptions;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;
[ApiController]
[Route("api/Create worker")]
public class WorkerRegisterController : ControllerBase
{
    private readonly IWorkerRegisterService _service;
    private readonly IAccountService _account;
    private readonly ISameUsersCheckService _check;

    public WorkerRegisterController(IWorkerRegisterService service, IAccountService account, ISameUsersCheckService check)
    {
        _service = service;
        _account = account;
        _check = check;
    }
    
    public CancellationToken CancellationToken => HttpContext.RequestAborted;
    
    [HttpPost]
    public async Task<ActionResult<WorkerDto>> CreateWorkerAsync([FromBody] CreateWorker model)
    {
        if (_check.CheckSameUsers(model.Login))
            throw new SameUsersException($"User with login {model.Login} already exists!");
        WorkerDto workerDto = await _service.CreateWorkerAsync(model.Name, model.SupervisorId, CancellationToken);
        await _account.CreateAccountAsync(model.Login, model.Password, workerDto.Id, CancellationToken);
        return Ok(workerDto);
    }
}