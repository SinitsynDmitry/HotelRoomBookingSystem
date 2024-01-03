/******************************************************************************
 *
 * File: Constants.cs
 *
 * Description: Constants.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

namespace HotelModels.Helpers
{
    public class Constants
    {
        public const string DBConnectionName = "DBConnection";
        public const string ApiConnectionName = "ApiConnection";

        public const string AdministratorsRole = "Administrators";
        public const string ManagersRole = "Managers";
        public const string DefaultRole = "Customer";


        public const string ApiKeyHeaderName = "X-API-Key";
        public const string ApiKeyName = "ApiKey";


        public const string CheckInTimeName = "CheckInTime";
        public const string CancellationLimitName = "CancellationLimit";
    }
}
