using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using CoinbaseApi.Exceptions;
using CoinbaseApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAuthCoinbase.Pages
{
    public class IndexModel : PageModel
    {
        public string CoinbaseId { get; set; }

        public string CoinbaseAvatar { get; set; }

        public string CoinbaseName { get; set; }

        public List<AccountDetail> AccountDetails { get; set; } = new List<AccountDetail>();

        public async Task OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                CoinbaseName = User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;
                CoinbaseId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                CoinbaseAvatar = User.FindFirst(c => c.Type == "urn:coinbase:avatar")?.Value;

                try
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    var coinbaseApi = new CoinbaseApi(accessToken);
                    var accounts = coinbaseApi.GetAccounts();
                    this.AccountDetails = accounts.Select(account => new AccountDetail
                    {
                        Account = account,
                        Addresses = coinbaseApi.GetAddresses(account)
                    }).ToList();

                }
                catch(TokenExpiredException)
                {
                    await HttpContext.SignOutAsync();
                }
            }
        }
    }
}
