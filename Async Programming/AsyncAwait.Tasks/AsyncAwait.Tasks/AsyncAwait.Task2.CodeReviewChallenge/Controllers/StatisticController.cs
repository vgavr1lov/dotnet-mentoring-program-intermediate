using CloudServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AsyncAwait.Task2.CodeReviewChallenge.Controllers
{
    public class StatisticController : Controller
    {
        private readonly IStatisticService _statisticService;

        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }
        [HttpGet]
        public async Task<ActionResult> GetStatistic(string path)
        {
            await Task.Delay(3000);
            var visitsCount = await _statisticService.GetVisitsCountAsync(path).ConfigureAwait(false);

            return PartialView("_StatisticPartial", visitsCount);  
        }

    }
}
