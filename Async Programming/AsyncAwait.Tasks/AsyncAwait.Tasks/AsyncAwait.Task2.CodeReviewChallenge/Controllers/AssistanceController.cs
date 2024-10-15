using CloudServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task2.CodeReviewChallenge.Controllers
{
    public class AssistanceController : Controller
    {
        private readonly ISupportService _supportService;

        public AssistanceController(ISupportService supportService)
        {
            _supportService = supportService ?? throw new ArgumentNullException(nameof(supportService));
        }

        [HttpGet]
        public async Task<ActionResult> GetSupportInfo(string requestInfo)
        {
            await Task.Delay(5000);
            var supportRequest = await _supportService.GetSupportInfoAsync(requestInfo)
                .ConfigureAwait(false);

            return PartialView("_SupportInfoPartial", supportRequest);
        }
    }
}
