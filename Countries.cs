namespace server;

using System.Runtime.CompilerServices;
using MySql.Data.MySqlClient;


class Countries
{
    public record GetAll_Data(int country_id, string name);
    public static async Task<List<GetAll_Data>>
    GetAll(Config config)
    {
        List<GetAll_Data> result = new();
        string query = "SELECT country_id, name FROM countries";
        using (var reader = await MySqlHelper.
        ExecuteReaderAsync(config.db, query))
        {
            while (reader.Read())
            {
                result.Add(new(reader.GetInt32(0),
                reader.GetString(1)));
            }
        }
        return result;
    }

    public record Get_Data(int country_id, string name);
    public static async Task<Get_Data?>

    Get(int id, Config config)
    {
        Get_Data? result = null;
        string query = "SELECT country_id, name FROM countries WHERE country_id = @id";
        var parameters = new MySqlParameter[] { new("@id", id) };
        using (var reader = await MySqlHelper.
        ExecuteReaderAsync(config.db, query, parameters))
        {
            if (reader.Read())
            {
                result = new(reader.GetInt32(0), reader.GetString(1));
            }
        }
        return result;
    }

    public record Post_Args(string Name);
    public static async Task

    Post(Post_Args country, Config config)
    {
        string query = """
            INSERT INTO countries(name)
            VALUES(@name)
            """;

        var parameters = new MySqlParameter[]
        {
                new("@name", country.Name)
        };

        await MySqlHelper.ExecuteNonQueryAsync(config.db, query, parameters);
    }

    public record Put_Args(string Name);
    public static async Task
    Put(int id, Put_Args country, Config config)
    {
        string query = """
            UPDATE countries
            SET name = @name
            WHERE country_id = @id
            """;

        var parameters = new MySqlParameter[]
        {
                new("@id", id),
                new("@name", country.Name),
        };

        await MySqlHelper.ExecuteNonQueryAsync(config.db, query, parameters);
    }

    public static async Task Delete(int id, Config config)
    {
        string query = "DELETE FROM countries WHERE country_id = @id";
        var parameters = new MySqlParameter[]
        {
            new ("@id", id)
        };
        await MySqlHelper.ExecuteNonQueryAsync(config.db, query, parameters);
    }
}