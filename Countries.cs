namespace server;

using System.Runtime.CompilerServices;
using MySql.Data.MySqlClient;


class Countries
{
    public record GetAll_Data(int country_id, string name);
    public static async Task<List<GetAll_Data>>
    GetAll(Config config)
    {
        List<GetAll_Data> result = new();           //Creates empty list to fill rows from the database
        string query = "SELECT country_id, name FROM countries";        //fetch all countries
        using (var reader = await MySqlHelper.
        ExecuteReaderAsync(config.db, query))           //
        {
            while (reader.Read())           //loops through every row 
            {
                result.Add(new(reader.GetInt32(0),      //reads column 0(country_id) as int and column 1 (name) as a string
                reader.GetString(1)));      //creates a new getall data record and adds to the list
            }
        }
        return result;      //returns list of all countries 
    }

    public static async Task<List<GetAll_Data>> Search(string? search, Config config)
    {
        // Om ingen sökterm → använd vanliga GetAll()
        if (string.IsNullOrEmpty(search))
        {
            return await GetAll(config);
        }

        List<GetAll_Data> result = new();

        string query = """
            SELECT country_id, name
            FROM countries
            WHERE name LIKE @search
        """;

        var parameters = new MySqlParameter[]
        {
            new("@search", "%" + search + "%")
        };

        using (var reader = await MySqlHelper.ExecuteReaderAsync(config.db, query, parameters))
        {
            while (reader.Read())
            {
                result.Add(new(reader.GetInt32(0), reader.GetString(1)));
            }
        }

        return result;
    }
    public record City_Data(int City_id, string Name);         //record for citys that belong to a country
    public record Get_Data(int country_id, string name, List<City_Data> Cities);
    public static async Task<Get_Data?>

    Get(int id, Config config)
    {
        Get_Data? result = null;        //assumes no country is found
        string query = """
        SELECT c.country_id, c.name, ci.city_id, ci.name 
        FROM countries c
        LEFT JOIN cities ci ON ci.country_id = c.country_id
        WHERE c.country_id = @id
        """;
        var parameters = new MySqlParameter[] { new("@id", id) };       //creates parameterlist 
        using (var reader = await MySqlHelper.
        ExecuteReaderAsync(config.db, query, parameters))
        {
            List<City_Data> cities = new();
            int countryId = 0;
            string countryName = "";

            while (reader.Read())
            {
                if (result is null)
                {
                    countryId = reader.GetInt32(0);
                    countryName = reader.GetString(1);
                }

                if (!reader.IsDBNull(2))
                {
                    int cityId = reader.GetInt32(2);
                    string cityName = reader.GetString(3);
                    cities.Add(new City_Data(cityId, cityName));
                }
                result = new(countryId, countryName, cities);
            }
        }
        return result;
    }

    public record Post_Args(string Name, int user_id);

    public static async Task Post(Post_Args country, Config config)
    {
        string query = """
        INSERT INTO countries(name)
        SELECT @name
        FROM users
        WHERE user_id = @user_id
          AND admin = 1
        """;

        var parameters = new MySqlParameter[]
        {
        new("@name", country.Name),
        new("@user_id", country.user_id)
        };

        int rows = await MySqlHelper.ExecuteNonQueryAsync(config.db, query, parameters);

        if (rows == 0)
            throw new UnauthorizedAccessException("Only an admin may add a country");
    }

    public record Put_Args(string Name, int user_id);

    public static async Task Put(int id, Put_Args country, Config config)
    {
        string query = """
        UPDATE countries
        SET name = @name
        WHERE country_id = @id
          AND EXISTS (
              SELECT 1 FROM users
              WHERE user_id = @user_id
                AND admin = 1
          )
        """;

        var parameters = new MySqlParameter[]
        {
        new("@id", id),
        new("@name", country.Name),
        new("@user_id", country.user_id)
        };

        int rows = await MySqlHelper.ExecuteNonQueryAsync(config.db, query, parameters);

        if (rows == 0)
            throw new UnauthorizedAccessException("Only an admin may update a coury");
    }

    public static async Task Delete(int id, int user_id, Config config)
    {
        string query = """
        DELETE c
        FROM countries c
        JOIN users u ON u.user_id = @user_id
        WHERE c.country_id = @id
          AND u.admin = 1
    """;

        var parameters = new MySqlParameter[]
        {
        new("@id", id),
        new("@user_id", user_id),
        };

        int rows = await MySqlHelper.ExecuteNonQueryAsync(config.db, query, parameters);

        if (rows == 0)
        {
            throw new UnauthorizedAccessException("Only an admin may delete a country");
        }
    }
}