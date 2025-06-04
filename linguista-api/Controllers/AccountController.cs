using linguista_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace linguista_api.Controllers
{
	public class AccountController: ControllerBase
	{
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
		{
			_accountService = accountService;
		}

        /// <summary>
        /// Get Account Details
        /// </summary>
        /// <param name="request"></param>
        /// <returns>AccountDetails</returns>
        [HttpGet("accountDetails/{accountId}")]
        [Authorize]
        public async Task<IActionResult> RetrieveAccountDetails([FromRoute] string accountId)
        {
            var response = await _accountService.GetAccountDetails(accountId);

            if (response == null)
            {
                return BadRequest("Valid response not provided");
            }

            return Ok(response);
        }
    }
}