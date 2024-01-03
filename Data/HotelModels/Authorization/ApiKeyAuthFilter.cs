/******************************************************************************
 *
 * File: ApiKeyAuthFilter.cs
 *
 * Description: ApiKeyAuthFilter.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using HotelModels.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HotelModels.Authorization
{
    public class ApiKeyAuthFilter : IAuthorizationFilter
    {
        private readonly IApiKeyValidation _apiKeyValidation;

        public ApiKeyAuthFilter(IApiKeyValidation apiKeyValidation)
        {
            _apiKeyValidation = apiKeyValidation;
        }

        /// <summary>
        /// Ons the authorization.
        /// </summary>
        /// <param name="context">The context.</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string userApiKey = context.HttpContext.Request.Headers[Constants.ApiKeyHeaderName].ToString();

            if (string.IsNullOrWhiteSpace(userApiKey))
            {
                context.Result = new BadRequestResult();
                return;
            }

            if (!_apiKeyValidation.IsValidApiKey(userApiKey))
                context.Result = new UnauthorizedResult();
        }
    }
}
