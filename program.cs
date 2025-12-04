using MySql.Data.MySqlClient;
using server;

var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD
Config config = new ("server=127.0.0.1;uid=root;pwd=kebab123;database=drinks_and_travel");
builder.Services.AddSingleton<Config>(config);
/*
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
*/
=======
Config config = new("server=127.0.0.1;uid=root;pwd=Mans010101!;database=drinks_and_travel");
builder.Services.AddSingleton<Config>(config);
//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
    //options.Cookie.HttpOnly = true;
    //options.Cookie.IsEssential = true;
//});

>>>>>>> f1c2c157e67d56361f2864e179a8c917da39a0d6
var app = builder.Build();
//app.UseSession();


//app.MapGet("/login", Login.Get);
//app.MapPost("/login", Login.Post);
//app.MapDelete("/login", Login.Delete);

app.MapGet("/users", Users.GetAll);
app.MapPost("/users", Users.Post);
app.MapGet("/users/{id}", Users.Get);
app.MapDelete("/users/{id}", Users.Delete);

// special, reset db
app.MapDelete("/db", db_reset_to_default);

app.Run();




async Task db_reset_to_default(Config config)
{

    // Drop all tables from database
    await MySqlHelper.ExecuteNonQueryAsync(config.ConnectionString, "DROP TABLE IF EXISTS users");

    // Create all tables
    string users_table = """
        CREATE TABLE users
        (
            id INT PRIMARY KEY AUTO_INCREMENT,
            email varchar(256) NOT NULL UNIQUE,
            password TEXT
        )
    """;
    await MySqlHelper.ExecuteNonQueryAsync(config.ConnectionString, users_table);
}

