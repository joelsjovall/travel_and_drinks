global using MySql.Data.MySqlClient;
using server;


Config config = new("server=127.0.0.1;uid=root;pwd=kebab123;database=drinks_and_travel");
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(config);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
   options.Cookie.IsEssential = true;
});

var app = builder.Build();
app.UseSession();
app.MapGet("/", () => "Hello world");

app.Run();

async Task db_reset_to_default()
{
    string create_users = """
    Create Table users
    (
    id INT PRIMARY KEY AUTO_INCERMENT,
    email VARCHAR(254) NOT NULL UNIQUE,
    password VARCHAR(128)
    
    )
""";
await MySqlHelper.ExecuteNonQueryAsync(config.db, "DROP TABEL IF EXISTS users"):
await MySqlHelper.ExecuteNonQueryAsync(config.db, create_users)
}


