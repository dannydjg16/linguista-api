using linguista_api.Models.Completions;
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

        /// <summary>
        /// Make a call to the Completions Endpoint, gpt 3.5-turbo
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("completions")]
        public async Task<IActionResult> CompletionsGpt35Turbo(CompletionsRequest request)
        {
            var response = await _completionsService.PrepareRequest(request);

            if(response == null)
            {
                return BadRequest("Valid response not provided");
            }

            return Ok(response);
        }

        /// <summary>
        /// Make a call to the Completions Endpoint, gpt 3.5-turbo
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("test")]
        public async Task<IActionResult> Test()
        { 
            return Ok("connection");
        }
    }
}