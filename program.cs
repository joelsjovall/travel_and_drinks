global using MySql.Data.MySqlClient;
using System.ComponentModel;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using server;

Config config = new("server=127.0.0.1;uid=root;pwd=kebab123;database=d_a_t");
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(config);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
    {
        options.Cookie.IsEssential = true;
        options.Cookie.HttpOnly = true;
    });
var app = builder.Build();
app.UseSession();

//authenticate
app.MapDelete("/login", Login.Delete);
app.MapGet("/profile", Profile.Get);
app.MapPost("/login", Login.Post);

// CRUD users
app.MapGet("users", Users.GetAll);                  //Get all users
app.MapGet("users/{id}", Users.Get);               //Get one user
app.MapPost("/users", Users.Post);                // Create a user
app.MapPut("/users/{id}", Users.Put);
app.MapDelete("/users/{id}", Users.Delete);
app.MapPost("/cities/search", async (CitySearchRequest request, Config config) =>
{
    return await Cities.Get(request.City, config);
});
app.MapGet("/cities", async (string? search, Config config) =>
{
    return await Cities.Get(search, config);
});

    //Delete a user 


//CRUD countries
app.MapGet("countries", Countries.GetAll);              //Get all countries
app.MapGet("/countries/{id}", Countries.Get);          //Get one country
app.MapPost("/countries", Countries.Post);            //Create/add a country
app.MapPut("/countries/{id}", Countries.Put);        //Update country
app.MapDelete("/delete/{id}", Countries.Delete);    //Delete country
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.Run();




