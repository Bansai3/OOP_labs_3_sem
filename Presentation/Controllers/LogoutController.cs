using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;
[ApiController]
[Route("api/Logout")]
public class LogoutController : ControllerBase
{
   
    private readonly ILogoutService _service;

    public LogoutController(ILogoutService service)
    {
        _service = service;
    }
    
    public CancellationToken CancellationToken => HttpContext.RequestAborted;
    
    [HttpPost]
    public void Logout([FromBody] CreateLogout model)
    {
        _service.Logout(model.Login, CancellationToken);
    }
}