using Microsoft.AspNetCore.Mvc;
using TravelTrek.Application.DTOs.Ner;
using TravelTrek.Application.Interfaces;

namespace TravelTrek.API.Controllers;

[Route("api/Ner")]
public class NerController : ApiBaseController
{
    private readonly INerService _nerService;

    public NerController(INerService nerService)
    {
        _nerService = nerService;
    }

    [HttpGet("extract")]
    public async Task<IActionResult> Extract([FromQuery] string query, CancellationToken ct)
    {
        var request = new NerRequest { Inputs = query };
        var result = await _nerService.ExtractAndParseTripDataAsync(request, ct);
        return ToActionResult(result);
    }
}
