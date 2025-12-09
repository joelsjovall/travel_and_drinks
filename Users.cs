namespace server;

using System.Runtime.CompilerServices;
using MySql.Data.MySqlClient;

class Users
{


    public record GetAll_Data(int Id, string Email, string Password);
    public static async Task<List<GetAll_Data>>
    GetAll(Config config)
    {
        List<GetAll_Data> result = new();
        string query = "SELECT user_id, email, password FROM users";
        using (var reader = await MySqlHelper.ExecuteReaderAsync(config.db, query))
        {
            while (reader.Read())
            {
                result.Add(new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
            }
        }
        return result;
    }

    public record Get_Data(string Email, string Password);
    public static async Task<Get_Data?>
    Get(int id, Config config)
    {
        Get_Data? result = null;
        string query = "SELECT email, password FROM users WHERE user_id = @id";
        var parameters = new MySqlParameter[] { new("@id", id) };
        using (var reader = await MySqlHelper.ExecuteReaderAsync(config.db, query, parameters))
        {
            if (reader.Read()) // using if instead of while since we only expect a single result from this query
            {
                result = new(reader.GetString(0), reader.GetString(1));
            }
        }
        return result;
    }


    public record Post_Args(string Email, string Password);
    public static async Task
    Post(Post_Args user, Config config)
    {
        string query = """
            INSERT INTO users(email, password)
            VALUES (@email, @password)
        """;

        var parameters = new MySqlParameter[]
        {
            new("@email", user.Email),
            new("@password", user.Password),    
        };

        await MySqlHelper.ExecuteNonQueryAsync(config.db, query, parameters);
    }

    public record Put_Args(string Email, string Password); //data we wish to pudate
    public static async Task 
    Put(int id, Put_Args user, Config config)
    {
        string query = """
            UPDATE users 
            SET email = @email, password = @password
            WHERE user_id = @id                                 //ge förklrlaing till all denna kod
            """;

        var parameters = new MySqlParameter[]
        {
                new("@id", id),
                new("@email", user.Email),
                new("@password", user.Password),
        };

        await MySqlHelper.ExecuteNonQueryAsync(config.db, query, parameters);
    }



    public static async Task 
    Delete(int id, Config config)
    {
        string query = "DELETE FROM users WHERE user_id = @id";
        var parameters = new MySqlParameter[] { new("@id", id) };

        await MySqlHelper.ExecuteNonQueryAsync(config.db, query, parameters);
    }
}
