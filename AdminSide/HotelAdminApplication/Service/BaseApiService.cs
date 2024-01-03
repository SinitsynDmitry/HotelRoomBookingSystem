/******************************************************************************
 *
 * File: BaseApiService.cs
 *
 * Description: BaseApiService.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using Azure.Core;
using HotelModels.Helpers;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Newtonsoft.Json;
using NuGet.Common;
using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;

namespace HotelAdminApplication.Service
{
    public class BaseApiService
    {
        protected readonly IHttpClientFactory _httpClientFactory;
        protected readonly Uri _basePath;
        protected readonly string _apiKey;

        public BaseApiService(IHttpClientFactory httpClientFactory, string apiKey, string basePath)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _basePath = new Uri(basePath);
            _apiKey = apiKey;
        }

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <returns>A HttpClient.</returns>
        protected HttpClient CreateClient()
        {
            HttpClient client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add(Constants.ApiKeyHeaderName, $"{_apiKey}");

            client.BaseAddress = _basePath;
            return client;
        }

        /// <summary>
        /// AS the p i_ post request async.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="apiUrl">The api url.</param>
        /// <param name="requestObject">The request object.</param>
        /// <returns>A Task.</returns>
        protected async Task<HttpResponseMessage> API_RequestAsync(HttpClient client, string apiUrl, object requestObject, HttpMethod httpMethod = HttpMethod.Post)
        {
            // Convert the login request object to JSON
            string jsonContent = JsonConvert.SerializeObject(requestObject);
            // Create StringContent with JSON media type
            using (HttpContent content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json"))
            {
                if(httpMethod == HttpMethod.Put)
                {
                    var responsePut = await client.PutAsync(apiUrl, content);

                    return responsePut;
                }
                var response = await client.PostAsync(apiUrl, content);

                return response;
            }
        }
    }
}
