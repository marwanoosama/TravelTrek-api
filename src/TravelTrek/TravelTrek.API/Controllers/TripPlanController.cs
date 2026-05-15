using Microsoft.AspNetCore.Mvc;
using TravelTrek.Application.DTOs.TripPlanner;
using TravelTrek.Application.Interfaces;

namespace TravelTrek.API.Controllers
{
    [Route("api/trip-plan")]
    public class TripPlanController : ApiBaseController
    {
        private readonly ITripPlanService _tripPlanService;

        public TripPlanController(ITripPlanService tripPlanService)
        {
            _tripPlanService = tripPlanService;
        }
        
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateTripPlan([FromBody] TripPlanRequest request, CancellationToken ct)
        {
            var result = await _tripPlanService.GenerateTripPlanAsync(request, ct);
            return ToActionResult(result);
        }
    }
}
