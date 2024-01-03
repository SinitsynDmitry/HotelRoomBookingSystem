/******************************************************************************
 *
 * File: ApiKeyAttribute.cs
 *
 * Description: ApiKeyAttribute.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using Microsoft.AspNetCore.Mvc;

namespace HotelModels.Authorization
{
    public class ApiKeyAttribute : ServiceFilterAttribute
    {
        public ApiKeyAttribute()
            : base(typeof(ApiKeyAuthFilter))
        {
        }
    }
}
