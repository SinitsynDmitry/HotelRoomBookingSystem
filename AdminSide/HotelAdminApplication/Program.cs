/******************************************************************************
 *
 * File: Program.cs
 *
 * Description: Program.cs class and he's methods.
 *
 * Copyright (C) 2024 by Dmitry Sinitsyn
 *
 * Date: 4.1.2024	 Authors:  Dmitry Sinitsyn
 *
 *****************************************************************************/

using HotelAdminApplication.Data;
using HotelAdminApplication.Service;
using HotelData;
using HotelModels;
using HotelModels.Helpers;
using HotelModels.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var checkInTime = builder.Configuration.GetValue<TimeSpan?>(Constants.CheckInTimeName) ?? throw new InvalidOperationException($"TimeSpan '{Constants.CheckInTimeName}' not found.");
var cancellationLimit = builder.Configuration.GetValue<int?>(Constants.CancellationLimitName) ?? throw new InvalidOperationException($"Int '{Constants.CancellationLimitName}' not found.");

#region Connection

var connectionDB = builder.Configuration.GetConnectionString(Constants.DBConnectionName) ?? throw new InvalidOperationException($"Connection string '{Constants.DBConnectionName}' not found.");
var connectionAPI = builder.Configuration.GetConnectionString(Constants.ApiConnectionName) ?? throw new InvalidOperationException($"Connection string '{Constants.ApiConnectionName}' not found.");
//from secret
var apiKey = builder.Configuration.GetValue<string>(Constants.ApiKeyName) ?? throw new InvalidOperationException($"String '{Constants.ApiKeyName}' not found.");

#endregion Connection

builder.Services.AddDbContext<Hotel_DbContext>(options => options.UseSqlServer(connectionDB));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddHttpClient();

builder.Services.AddDefaultIdentity<CustomerUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedEmail = false;

}).AddRoles<IdentityRole>().AddEntityFrameworkStores<Hotel_DbContext>();

builder.Services.AddCors();
builder.Services.AddDistributedMemoryCache();

#region APIs

builder.Services.AddSingleton<IBookingsAdminRepository>(provider =>
{
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    return new BookingService(httpClientFactory, apiKey, connectionAPI);
});

builder.Services.AddSingleton<IRoomsAdminRepository>(provider =>
{
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    return new RoomsService(httpClientFactory, apiKey, connectionAPI);
});

#endregion APIs

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<Hotel_DbContext>();
    context.Database.Migrate();
    // requires using Microsoft.Extensions.Configuration;
    // Set password with the Secret Manager tool.
    // dotnet user-secrets set SeedUserPW <pw>
    //Passwords must have at least one non alphanumeric character.
    //Passwords must have at least one lowercase('a' - 'z').
    //Passwords must have at least one uppercase('A' - 'Z').

    var testUserPw = builder.Configuration.GetValue<string>("SeedUserPW");

    await SeedData.Initialize(services, testUserPw);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Bookings}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
