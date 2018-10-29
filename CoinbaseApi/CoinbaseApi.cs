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

        #region Coinbase Api Methods

        public TransactionReciept SendCurrency(string accountId, double amount, string toAddress, Currency currency, string twoFactor ="", string idem = "")
        {
            var request = new RestRequest($"accounts/{accountId}/transactions", Method.POST, DataFormat.Json);
            request.AddParameter("type", "send");
            request.AddParameter("to", toAddress);
            request.AddParameter("amount", amount);
            request.AddParameter("currency", currency.ToString());

            if(false == string.IsNullOrWhiteSpace(idem))
            {
                request.AddParameter("idem", idem);
            }

            if(false == string.IsNullOrWhiteSpace(twoFactor))
            {
                request.AddParameter("CB-2FA-TOKEN", twoFactor);
            }

            var requestString = this.LogRequest(request);

            var response = this._client.Execute<CoinbaseResponseWrapper<TransactionReciept>>(request);
            this.HandleKnownExceptions(response);
            return response.Data?.Data;
        }

        private string LogRequest(IRestRequest request)
        {
            var requestToLog = new
            {
                resource = request.Resource,
                // Parameters are custom anonymous objects in order to have the parameter type as a nice string
                // otherwise it will just show the enum value
                parameters = request.Parameters.Select(parameter => new
                {
                    name = parameter.Name,
                    value = parameter.Value,
                    type = parameter.Type.ToString()
                }),
                // ToString() here to have the method as a nice string otherwise it will just show the enum value
                method = request.Method.ToString(),
                // This will generate the actual Uri used in the request
                uri = this._client.BuildUri(request),
            };

            return JsonConvert.SerializeObject(requestToLog);
        }

        public List<Account> GetAccounts()
        {
            var request = new RestRequest("accounts/", Method.GET, DataFormat.Json);
            var response = this._client.Execute<CoinbaseResponseWrapper<List<Account>>>(request);

            this.HandleKnownExceptions(response);
            return response.Data?.Data;
        }

        public List<AccountAddress> GetAddresses(string Id)
        {
            var request = new RestRequest($"accounts/{Id}/addresses", Method.GET, DataFormat.Json);
            var response = this._client.Execute<CoinbaseResponseWrapper<List<AccountAddress>>>(request);

            this.HandleKnownExceptions(response);
            return response.Data?.Data;
        }
        public List<AccountAddress> GetAddresses(Account account) => this.GetAddresses(account.Id);

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
