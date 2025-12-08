<<<<<<< HEAD
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


=======
namespace server;

static class Login
{
    public static void Delete(HttpContext ctx)
    {
        if (ctx.Session.IsAvailable)
        {
            ctx.Session.Clear();
        }
    }
    public record Post_Args(string Email, string Password);
    public static async Task<bool>
    Post(Post_Args credentials, Config config, HttpContext ctx)
    {
        bool result = false;
        string query = "SELECT id FROM users WHERE email = @email AND password = @password";
        var parameters = new MySqlParameter[]
        {
            new("email", credentials.Email),
            new("password", credentials.Password),
        };

        object query_result = await MySqlHelper.ExecuteScalarAsync(config.db, query, parameters);
        if (query_result is int id)
        {
            if (ctx.Session.IsAvailable)
            {
                ctx.Session.SetInt32("user_id", id);
                result = true;
            }
        }

        return result;
    }
}
>>>>>>> 7b22cea96aa087b9fda96eceabafc015f1ef541a
