namespace server;

using System.Runtime.CompilerServices;
using MySql.Data.MySqlClient;
public record CitySearchRequest(string? City);


class Cities
{
 public record GetAll_Data(int city_id, int country_id, string name);

    public static async Task<List<GetAll_Data>> 
    Get(string? searchTerm, Config config)
    {
        List<GetAll_Data> result = new();

        string query;
        MySqlParameter[] parameters;

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            // Om sökterm finns använd LIKE
            query = "SELECT city_id, country_id, name FROM cities WHERE name LIKE @search";
            parameters = new MySqlParameter[] { new("@search", "%" + searchTerm + "%") };
        }
        else
        {
            // Ingen sökterm hämta alla
            query = "SELECT city_id, country_id, name FROM cities";
            parameters = Array.Empty<MySqlParameter>();
        }

        using (var reader = await MySqlHelper.ExecuteReaderAsync(config.db, query, parameters))
        {
            while (reader.Read())
            {
                result.Add(new(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetString(2)
                ));
            }
        }

        return result;
    }




}