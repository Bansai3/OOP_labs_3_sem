using Application.Dto;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;
[ApiController]
[Route("api/Process messages")]
public class HandleMessagesController : ControllerBase
{
    private readonly IHandleMessagesService _service;

    public HandleMessagesController(IHandleMessagesService service)
    {
        _service = service;
    }
    
    public CancellationToken CancellationToken => HttpContext.RequestAborted;
    
    [HttpPost]
    public async Task<ActionResult<MessageHandlerDto>> HandleMessagesAsync([FromBody] CreateMessagesHandler model)
    {
        MessageHandlerDto messageHandlerDto = await _service.HandleMessagesAsync(model.WorkerId, CancellationToken);
        return Ok(messageHandlerDto);
    }
}