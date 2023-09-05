using Application.Dto;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;
[ApiController]
[Route("api/Create message")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _service;

    public MessageController(IMessageService service)
    {
        _service = service;
    }
    
    public CancellationToken CancellationToken => HttpContext.RequestAborted;
    
    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessageAsync([FromBody] CreateMessage model)
    {
        MessageDto messageDto = await _service.CreateMessageAsync(model.MessageSourceId, model.Text, CancellationToken);
        return Ok(messageDto);
    }
}