using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinbaseApi.Exceptions;
using CoinbaseApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace OAuthCoinbase.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        public AccountController()
        {
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect(Url.Content("~/"));
        }

        [HttpPost]
        public async Task<TransactionReciept> SendMoney(string accountId, string toAddress, Currency currency, 
                                    double amount = .001, string twoFactor = "", string description = "", string idem = "")
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var coinbaseApi = new CoinbaseApi(accessToken);
                return coinbaseApi.SendCurrency(accountId, amount, toAddress, currency, description,  twoFactor, idem);
            }
            catch(Transaction2FaRequiredException)
            {
                return null;
            }
        }
    }
}