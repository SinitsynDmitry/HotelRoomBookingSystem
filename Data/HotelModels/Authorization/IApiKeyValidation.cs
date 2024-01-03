/******************************************************************************
 *
 * File: IApiKeyValidation.cs
 *
 * Description: IApiKeyValidation.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

namespace HotelModels.Authorization
{
    public interface IApiKeyValidation
    {
        /// <summary>
        /// Are the valid api key.
        /// </summary>
        /// <param name="userApiKey">The user api key.</param>
        /// <returns>A bool.</returns>
        bool IsValidApiKey(string userApiKey);
    }
}
