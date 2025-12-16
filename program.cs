global using MySql.Data.MySqlClient;
using System.ComponentModel;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using server;


Config config = new("server=127.0.0.1;uid=root;pwd=Mans010101!;database=dranks_and_travel");
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

app.MapPost("/hotels", async (Hotels.CreateHotel_Data data, Config config) =>
{
    int id = await Hotels.Create(data, config);
    return Results.Ok(new { hotel_id = id });
});

app.MapPut("/hotels/{id}", async (int id, Hotels.UpdateHotel_Data data, Config config) =>
{
    bool updated = await Hotels.Update(id, data, config);
    return updated ? Results.Ok() : Results.NotFound();
});

app.MapDelete("/hotels/{id}", async (int id, Config config) =>
{
    bool deleted = await Hotels.Delete(id, config);
    return deleted ? Results.Ok() : Results.NotFound();
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
app.MapPut("/events/{id}", Events.Put);
app.MapDelete("/events/{id}", Events.Delete);

app.MapGet("bookings", Bookings.Search);
app.MapGet("/bookings/joined", Bookings.GetAllJoined);   //Get all with users hotel and events with names and not only ids
app.MapPost("/bookings", Bookings.Post);
app.MapPut("/bookings/{id}", Bookings.Put);
app.MapDelete("/bookings/{id}", Bookings.Delete);

//Hotel ratings
// app.MapGet("/hotels/{hotelId}ratings", HotelRatings.GetByHotel);
// app.MapGet("/hotels/{hotelId}/ratings", HotelRatings.Post);

// fetch all ratings(hotels)
app.MapGet("/hotels/{hotelId}/ratings", async (int hotelId, Config config) =>
{
    return await HotelRatings.GetByHotel(hotelId, config);
});

// Create new rating(hotels)
app.MapPost("/hotels/{hotelId}/ratings", async (int hotelId, HotelRatings.Post_Args rating, Config config) =>
{
    await HotelRatings.Post(hotelId, rating, config);
    return Results.Ok();
});

//fetch all ratings(events)
app.MapGet("/events/{eventId}/ratings", async (int eventId, Config config) =>
{
    return await EventRatings.GetByEvent(eventId, config);
});

// Create new rating(events)
app.MapPost("/events/{eventId}/ratings", async (int eventId, EventRatings.Post_Args rating, Config config) =>
{
    await EventRatings.Post(eventId, rating, config);
    return Results.Ok();
});
//denna typ av kod används för att i ratings endpoints har flera parameters, och minimal api behöver veta exakt var varje parameter kommer ifrån, därför kan man ej ha mapPost här i ratings.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.Run();





