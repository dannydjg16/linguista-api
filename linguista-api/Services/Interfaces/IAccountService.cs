using System;
using linguista_api.Models.Account;

namespace linguista_api.Services.Interfaces
{
	public interface IAccountService
	{
		Task<AccountDetails> GetAccountDetails(string accountId);
	}
}