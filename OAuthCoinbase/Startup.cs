using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace OAuthCoinbase
{
    public class Startup
    {
        public const string COINBASE_AUTH_ID = "coinbase";

        public static readonly List<string> COINBASE_SCOPES = new List<string> {
            "wallet:accounts:read",
            "wallet:addresses:read",
            "wallet:buys:read",
            "wallet:checkouts:read",
            "wallet:contacts:read",
            "wallet:notifications:read",
            "wallet:orders:read",
            "wallet:transactions:read",
            "wallet:transactions:request",
            "wallet:transactions:send",
            "wallet:transactions:transfer",
            "wallet:user:email",
            "wallet:user:read",
            "wallet:withdrawals:create",
        };

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = COINBASE_AUTH_ID;
            })
            .AddCookie()
            .AddOAuth(COINBASE_AUTH_ID, options =>
            {
                options.ClientId = Configuration["Coinbase:ClientId"];
                options.ClientSecret = Configuration["Coinbase:ClientSecret"];
                options.CallbackPath = new PathString("/signin-coinbase");

                options.AuthorizationEndpoint = "https://www.coinbase.com/oauth/authorize?meta[send_limit_amount]=1";
                options.TokenEndpoint = "https://api.coinbase.com/oauth/token";
                options.UserInformationEndpoint = "https://api.coinbase.com/v2/user";

                COINBASE_SCOPES.ForEach(scope => options.Scope.Add(scope));

                options.SaveTokens = true;


                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                options.ClaimActions.MapJsonKey("urn:coinbase:avatar", "avatar_url");

                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                        request.Headers.Add("CB-VERSION", DateTime.Now.ToShortDateString());
                        var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();

                        var userData = JObject.Parse(await response.Content.ReadAsStringAsync());

                        var user = userData["data"];

                        context.RunClaimActions(JObject.FromObject(user));
                    }
                };
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();

            app.UseMvc();
        }
    }
}
