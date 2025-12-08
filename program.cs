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

app.MapGet("/", () => "Hello World!");
app.MapGet("/profile", Profile.Get);
app.MapDelete("/db", db_reset_to_defaut);
app.MapPost("/login", Login.Post);
app.MapDelete("/login", Login.Delete);
app.MapGet("/users", Users.GetAll);
app.MapPost("/users", Users.Post);
app.MapGet("/users/{id}", Users.Get);
app.MapDelete("/users/{id}", Users.Delete);




if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.Run();

async Task db_reset_to_defaut(Config config)
{
    string create_users = """
    CREATE TABLE users
    (
    id INT PRIMARY KEY AUTO_INCREMENT,
    email VARCHAR(100) NOT NULL UNIQUE,
    password VARCHAR(128),
    name VARCHAR(255)
    )
    """;
    await MySqlHelper.ExecuteNonQueryAsync(config.db, "DROP TABLE IF EXISTS users");
    await MySqlHelper.ExecuteNonQueryAsync(config.db, create_users);
    await MySqlHelper.ExecuteNonQueryAsync(config.db, "INSERT INTO users(email, password) VALUES ('joel.sjovall.com', '123')");
    await MySqlHelper.ExecuteNonQueryAsync(config.db, "INSERT INTO users(email, password, name) VALUES ('mans.oskarsson.com', '123'), Måns");
}


