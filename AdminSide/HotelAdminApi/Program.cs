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

using HotelData;
using HotelModels.Helpers;
using HotelModels.Authorization;
using HotelModels.Interfaces;
using Microsoft.EntityFrameworkCore;
using HotelAdminApi.Repository;

var builder = WebApplication.CreateBuilder(args);

#region Connection

var connectionDB = builder.Configuration.GetConnectionString(Constants.DBConnectionName) ?? throw new InvalidOperationException($"Connection string '{Constants.DBConnectionName}' not found.");
//from secret
var apiKey = builder.Configuration.GetValue<string>(Constants.ApiKeyName) ?? throw new InvalidOperationException($"String '{Constants.ApiKeyName}' not found.");

#endregion Connection

// Add services to the container.
builder.Services.AddSingleton(builder.Configuration);
builder.Services.AddDbContext<Hotel_DbContext>(options => options.UseSqlServer(connectionDB));

builder.Services.AddTransient<IApiKeyValidation>(provider =>
{
    return new ApiKeyValidation(apiKey);
});
builder.Services.AddScoped<ApiKeyAuthFilter>();

builder.Services.AddScoped<IRoomsAdminRepository, RoomsRepository>();
builder.Services.AddScoped<IBookingsAdminRepository, BookingsRepository>();

builder.Services.AddCors();
builder.Services.AddDistributedMemoryCache();

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
