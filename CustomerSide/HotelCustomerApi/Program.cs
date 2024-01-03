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

using HotelCustomerApi.Repository;
using HotelData;
using HotelModels.Authorization;
using HotelModels.Helpers;
using HotelModels.Interfaces;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

#region Connection

var connectionString = builder.Configuration.GetConnectionString(Constants.DBConnectionName) ?? throw new InvalidOperationException($"Connection string '{Constants.DBConnectionName}' not found.");

//from secret
var apiKey = builder.Configuration.GetValue<string>(Constants.ApiKeyName) ?? throw new InvalidOperationException($"String '{Constants.ApiKeyName}' not found.");

#endregion Connection

var checkInTime = builder.Configuration.GetValue<TimeSpan?>(Constants.CheckInTimeName) ?? throw new InvalidOperationException($"TimeSpan '{Constants.CheckInTimeName}' not found.");
var cancellationLimit = builder.Configuration.GetValue<int?>(Constants.CancellationLimitName) ?? throw new InvalidOperationException($"Int '{Constants.CancellationLimitName}' not found.");


builder.Services.AddSingleton(builder.Configuration);
builder.Services.AddDbContext<Hotel_DbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddCors();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddTransient<IApiKeyValidation>(provider =>
{
    return new ApiKeyValidation(apiKey);
});
builder.Services.AddScoped<ApiKeyAuthFilter>();


builder.Services.AddScoped<IRoomsRepository,RoomsRepository>();

builder.Services.AddScoped<IBookingsRepository, BookingsRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
