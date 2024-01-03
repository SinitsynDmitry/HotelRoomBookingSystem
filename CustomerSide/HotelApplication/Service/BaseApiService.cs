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
using Newtonsoft.Json;
using NuGet.Common;

namespace HotelApplication.Service
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
        /// the API post request async.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="apiUrl">The api url.</param>
        /// <param name="requestObject">The request object.</param>
        /// <returns>A Task.</returns>
        protected async Task<HttpResponseMessage> API_PostRequestAsync(HttpClient client, string apiUrl, object requestObject)
        {

            string jsonContent = JsonConvert.SerializeObject(requestObject);

            using (HttpContent content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json"))
            {

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                return response;
            }
        }
    }
}
