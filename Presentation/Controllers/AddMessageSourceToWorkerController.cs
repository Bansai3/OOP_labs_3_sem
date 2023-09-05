using Application.Dto;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;
[ApiController]
[Route("api/Add message source to worker")]
public class AddMessageSourceToWorkerController : ControllerBase
{
    private readonly IAddMessageSourceToWorkerService _service;

    public AddMessageSourceToWorkerController(IAddMessageSourceToWorkerService service)
    {
        _service = service;
    }
    
    public CancellationToken CancellationToken => HttpContext.RequestAborted;
    
    [HttpPost]
    public async Task<ActionResult<MessageSourceConnectionDto>> AddMessageSourceToWorkerAsync([FromBody] CreateMessageSourceConnectionToWorker model)
    {
        MessageSourceConnectionDto messageSourceToWorkerConnection = await _service.AddMessageSourceToWorkerAsync(model.WorkerId, model.MessageSourceId, CancellationToken);
        return Ok(messageSourceToWorkerConnection);
    }
}