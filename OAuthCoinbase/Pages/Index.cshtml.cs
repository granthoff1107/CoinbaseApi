using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
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

        public async Task OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                CoinbaseName = User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;
                CoinbaseId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                CoinbaseAvatar = User.FindFirst(c => c.Type == "urn:coinbase:avatar")?.Value;

                var accessToken = await HttpContext.GetTokenAsync("access_token");

                var coinbaseApi = new CoinbaseApi(accessToken);
                var accounts = coinbaseApi.GetAccounts();
            }
        }
    }
}
