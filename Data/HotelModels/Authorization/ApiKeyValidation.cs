/******************************************************************************
 *
 * File: ApiKeyValidation.cs
 *
 * Description: ApiKeyValidation.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

namespace HotelModels.Authorization
{
    public class ApiKeyValidation : IApiKeyValidation
    {
        private readonly string _configurationKey;
        public ApiKeyValidation(string configurationKey)
        {
            _configurationKey = configurationKey;
        }
        /// <summary>
        /// Are the valid api key.
        /// </summary>
        /// <param name="userApiKey">The user api key.</param>
        /// <returns>A bool.</returns>
        public bool IsValidApiKey(string userApiKey)
        {
            if (string.IsNullOrWhiteSpace(userApiKey))
                return false;
            string? apiKey = _configurationKey;
            if (apiKey == null || apiKey != userApiKey)
                return false;
            return true;
        }
    }
}
