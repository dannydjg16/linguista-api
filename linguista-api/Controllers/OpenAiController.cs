using linguista_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace linguista_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OpenAiController : ControllerBase
    {
        private readonly ICompletionsService _completionsService;

        public OpenAiController(ICompletionsService completionsService)
        {
            _completionsService = completionsService;
        }

        [HttpGet("completions")]
        public async Task<IActionResult> CompletionsGpt35Turbo()
        {
            var response = await _completionsService.PrepareRequest();
            return Ok(response);
        }
    }
}