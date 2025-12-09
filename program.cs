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
app.MapGet("/", () => "Hello World!");
app.MapDelete("/login", Login.Delete);
app.MapGet("/profile", Profile.Get);
app.MapPost("/login", Login.Post);

// CRUD users
app.MapGet("users", Users.GetAll);                  //Get all users
app.MapGet("users/{id}", Users.Get);               //Get one user
app.MapPost("/users", Users.Post);                // Create a user
app.MapPut("/users/{id}", Users.Put);            // Update a user 
app.MapDelete("/users/{id}", Users.Delete);     //Delete a user 

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.Run();




