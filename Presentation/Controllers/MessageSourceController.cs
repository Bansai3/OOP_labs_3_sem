using Application.Dto;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;

[ApiController]
[Route("api/Create message source")]
public class MessageSourceController : ControllerBase
{
    private readonly IMessageSourceService _service;

    public MessageSourceController(IMessageSourceService service)
    {
        _service = service;
    }
    
    public CancellationToken CancellationToken => HttpContext.RequestAborted;
    
    [HttpPost]
    public async Task<ActionResult<MessageSourceDto>> CreateMessageSourceAsync([FromBody] CreateMessageSource model)
    {
        MessageSourceDto accountDto = await _service.CreateMessageSourceAsync(model.Type, CancellationToken);
        return Ok(accountDto);
    }
}