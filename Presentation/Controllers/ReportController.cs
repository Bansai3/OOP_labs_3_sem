using Application.Dto;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;
[ApiController]
[Route("api/Create report")]
public class ReportController : ControllerBase
{
    private readonly IReportService _service;

    public ReportController(IReportService service)
    {
        _service = service;
    }
    
    public CancellationToken CancellationToken => HttpContext.RequestAborted;
    
    [HttpPost]
    public async Task<ActionResult<ReportDto>> CreateReportAsync([FromBody] CreateReport model)
    {
        ReportDto reportDto = await _service.CreateReportAsync(model.SupervisorId, model.TimePeriodInDays, CancellationToken);
        return Ok(reportDto);
    }
}