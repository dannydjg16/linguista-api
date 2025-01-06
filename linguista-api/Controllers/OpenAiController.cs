using linguista_api.Models.Completions;
using linguista_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using linguista_api.Models.TTS;

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
        //[Authorize]
        public async Task<IActionResult> CompletionsGpt35Turbo(CompletionsRequest request)
        {
            var response = await _completionsService.PrepareRequestReturnObject(request);

            if(response == null)
            {
                return BadRequest("Valid response not provided");
            }

            return Ok(response);
        }

        /// <summary>
        /// Make a call to the Text To Speech Endpoint
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("tts")]
        [Authorize]
        public async Task<IActionResult> GenerateTextToSpeech(TtsRequest request)
        {
            var response = await _completionsService.GenerateTextToSpeech(request);

            if (response == null)
            {
                return BadRequest("Valid response not provided");
            }

            return File(response, "audio/mpeg");
        }

        /// <summary>
        /// Make a call to Completions, then use whats returned to make and return TTS
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("completions/tts")]
        //[Authorize]
        public async Task<IActionResult> GetCompletionAndTts(CompletionsRequestWithTts request)
        {
            var response = await _completionsService.GetCompletionAndTts(request);

            if (response == null)
            {
                return BadRequest("Valid response not provided");
            }

            return Ok(response);
            //return File(response, "audio/mpeg");
        }

        /// <summary>
        /// Make a call to the test endpoint
        /// </summary>
        /// <returns></returns>
        [HttpPost("test")]
        [Authorize]
        public IActionResult Test()
        { 
            return Ok("connection");
        }
    }
}