<!-- README.md -->
This is the English version of the README.

[Eesti keeles](README.et.md)

# Hotel registration and cancellation system.
## General.

The system consists of two parts that work with a common database.

## Application for administration(HotelAdminApplication).

Users with access level "**Manager**" can:

- Add space
    
- Change room data
    
- Remove space
    
- View bookings
    
- View customer data

The system allows users with the access level "**Administrator**" to perform all actions of the "**Manager**" level and to add **new system users** to any access levels.

## Application for customers (HotelApplication).

To use the system, the customer must register and log in to the system.

The system allows users to search for available rooms by filtering time period, number of beds and price.

By selecting a room, the user can view it in more detail and book it for the required time period.

Also, everyone can check **their reservations**.
  **The reservation cannot be canceled later than 3 days before the start of the stay**.

## Installation.

File: appsettings.json

- "**DBConnection**" - Database connection string
    
- "**ApiConnection**" - Connecting to an API

For normal operation, it is necessary to maintain an "ApiKey" for each pair.

- AdminSide\HotelAdminApi
    
- AdminSide\HotelAdminApplication
and
- CustomerSide\HotelApplication
    
- CustomerSide\HotelCustomerApi

### For example:
     cd AdminSide\HotelAdminApi
     dotnet user-secrets init
     dotnet user-secrets set "ApiKey" "6CBxzdYcEgNDrRhMbDpkBF7e4d4Kib46dwL9ZE5egiL0iL5Y3dzREUBSUYVUwUkN"
    
     cd AdminSide\HotelAdminApi
     dotnet user-secrets init
     dotnet user-secrets set "ApiKey" "6CBxzdYcEgNDrRhMbDpkBF7e4d4Kib46dwL9ZE5egiL0iL5Y3dzREUBSUYVUwUkN"
    
     cd CustomerSide\HotelApplication
     dotnet user-secrets init
     dotnet user-secrets set "ApiKey" "YourSuperPassword"
      
     cd CustomerSide\HotelCustomerApi
     dotnet user-secrets init
     dotnet user-secrets set "ApiKey" "YourSuperPassword"

The system creates two system users **with a common password** at the first startup.

admin@hotels.com,
manager@hotels.com

You also need to store a **password** for them.

*Password must be at least 6 characters long.
Password must be at least one non-alphanumeric character.
Password must contain at least one lowercase letter ('a'-'z').
Password must contain at least one uppercase letter (A-Z).*

### For example:

     cd AdminSide\HotelAdminApplication
     dotnet user-secrets set SeedUserPW "#6CBxzdYcEgNDr"

   Everything is ready, start the admin part(**HotelAdminApplication** and **HotelAdminApi**).

( admin@hotels.com - "#6CBxzdYcEgNDr" )

Go to the portal(**HotelAdminApplication**) and create some "Room".

Now by entering the "**HotelApplication**" portal you can select these rooms and make reservations.