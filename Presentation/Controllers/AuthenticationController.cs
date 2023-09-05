using Application.Dto;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;
[ApiController]
[Route("api/Login")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _service;

    public AuthenticationController(IAuthenticationService service)
    {
        _service = service;
    }
    
    public CancellationToken CancellationToken => HttpContext.RequestAborted;
    
    [HttpPost]
    public async Task<ActionResult<AccountDto>> CheckAuthenticationAsync([FromBody] CreateAuthentication model)
    {
        AccountDto accountDto = await _service.CheckAuthenticationAsync(model.Login, model.Password, CancellationToken);
        return Ok(accountDto);
    }
}