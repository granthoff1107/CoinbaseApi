using CoinbaseApi.Exceptions;
using CoinbaseApi.Models;
using CoinbaseApi.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OAuthCoinbase
{
    public class CoinbaseApi
    {
        #region Members

        private RestClient _client;

        #endregion

        public JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        #region Constructors

        public CoinbaseApi(string accessToken, string coinbaseApiUrl = "https://api.coinbase.com/v2/")
        {
            this._client = this.CreateClient(accessToken, coinbaseApiUrl);
        }

        #endregion

        #region Protected Methods

        protected RestClient CreateClient(string accessToken, string coinbaseApiUrl = "https://api.coinbase.com/v2/")
        {
            var client = new RestClient(coinbaseApiUrl);
            this.SetJsonHandler(client);
            client.AddDefaultHeader("Authorization", $"Bearer {accessToken}");
            client.AddDefaultHeader("Accept", "application/json");
            client.AddDefaultHeader("CB-VERSION", DateTime.Now.ToShortDateString());

            return client;
        }

        protected void SetJsonHandler(IRestClient client)
        {
            var jsonHandlerType = "application/json";
            client.ClearHandlers();
            var jsonDeserializer = new JsonNetDeseralizer(JsonSettings);
            client.AddHandler(jsonHandlerType, jsonDeserializer);
        }
        #endregion

        #region Coinbase Api Methods

        public List<Account> GetAccounts()
        {
            var request = new RestRequest("accounts/", Method.GET, DataFormat.Json);
            var response = this._client.Execute<CoinbaseResponseWrapper<List<Account>>>(request);

            this.HandleKnownExceptions(response);
            return response.Data?.Data;
        }

        public List<AccountAddress> GetAddresses(Account account) => this.GetAddresses(account.Id);

        public List<AccountAddress> GetAddresses(string Id)
        {
            var request = new RestRequest($"accounts/{Id}/addresses", Method.GET, DataFormat.Json);
            var response = this._client.Execute<CoinbaseResponseWrapper<List<AccountAddress>>>(request);

            this.HandleKnownExceptions(response);
            return response.Data?.Data;
        }


        protected void HandleKnownExceptions(IRestResponse response)
        {
            if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                //TODO: Parse Error From Response
                throw new TokenExpiredException();
                //{ "errors":[{"id":"expired_token","message":"The access token expired"}]}
            }
            else if(response.StatusCode == HttpStatusCode.Forbidden)
            {
                //TODO: Parse Error From Response
                throw new InvalidScopeException();
                //{ "errors":[{"id":"invalid_scope","message":"Invalid scope","url":"https://developers.coinbase.com/api#permissions-scopes"}]}
            }

        }


        #endregion

    }
}
