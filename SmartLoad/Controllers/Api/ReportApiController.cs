using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLoad.Data;
using SmartLoad.Models;

[Route("api/[controller]")]
[ApiController]
public class ReportApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ReportApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("savescreenshot")]
    public async Task<IActionResult> SaveScreenshot([FromBody] ScreenshotRequest request)
    {
        var step = await _context.BlockPlacementSteps
            .FirstOrDefaultAsync(s => s.SchemeId == request.SchemeId && s.StepNumber == request.StepNumber);

        if (step == null)
        {
            step = new BlockPlacementStep
            {
                SchemeId = request.SchemeId,
                StepNumber = request.StepNumber,
                ScreenshotBase64 = request.Data
            };
            _context.BlockPlacementSteps.Add(step);
        }
        else
        {
            step.ScreenshotBase64 = request.Data;
        }

        await _context.SaveChangesAsync();
        return Ok(new { success = true, message = "Скриншот сохранён" });
    }
}

public class ScreenshotRequest
{
    public int SchemeId { get; set; }
    public int StepNumber { get; set; }
    public string Data { get; set; }
}