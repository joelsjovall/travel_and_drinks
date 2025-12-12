global using MySql.Data.MySqlClient;
using System.ComponentModel;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using server;


<<<<<<< HEAD
Config config = new("server=127.0.0.1;uid=root;pwd=kebab123;database=d_a_t");
=======
Config config = new("server=127.0.0.1;uid=root;pwd=Mans010101!;database=dranks_and_travel");
>>>>>>> 1590a98f983fabcf5519d83a3f413ed5af6a8425
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
app.MapGet("/cities", async (string? search, Config config) =>
{
    return await Cities.Get(search, config);
});
app.MapPost("/cities", Cities.Post);            //Create/add a city
app.MapPut("/cities", Cities.Put);        //Update city
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// HOTELS
app.MapGet("/hotels", async (
    int? country_id,
    int? city_id,
    string? search,
    Config config) =>
{
    return await Hotels.Get(country_id, city_id, search, config);
});

//Delete a user 


//CRUD countries
app.MapGet("countries", Countries.Search);              //Get all countries/search all countries
app.MapGet("/countries/{id}", Countries.Get);          //Get one country
app.MapPost("/countries", Countries.Post);            //Create/add a country
app.MapPut("/countries/{id}", Countries.Put);        //Update country
app.MapDelete("/delete/{id}", Countries.Delete);    //Delete country

//CRUD Events
app.MapGet("events", Events.Search);
app.MapGet("/events/{id}", Events.Get);
app.MapPost("/events", Events.Post);
app.MapPut("/events/{id}",Events.Put);
app.MapDelete("/events/{id}", Events.Delete);

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.Run();





