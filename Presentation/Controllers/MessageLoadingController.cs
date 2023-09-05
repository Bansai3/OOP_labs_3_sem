using Application.Dto;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;
[ApiController]
[Route("api/Load messages")]
public class MessageLoadingController : ControllerBase
{
    private readonly IMessageLoadingService _service;

    public MessageLoadingController(IMessageLoadingService service)
    {
        _service = service;
    }
    
    public CancellationToken CancellationToken => HttpContext.RequestAborted;
    
    [HttpPost]
    public async Task<ActionResult<MessageLoadingDto>> LoadMessagesFromMessageSourceAsync([FromBody] CreateMessagesLoading model)
    {
        MessageLoadingDto messageLoadingDto = await _service.LoadMessagesFromMessageSourceAsync(model.MessageSourceId, CancellationToken);
        return Ok(messageLoadingDto);
    }
}