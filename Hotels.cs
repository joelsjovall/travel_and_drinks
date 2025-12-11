namespace server;
using System.Runtime.CompilerServices;
using MySql.Data.MySqlClient;

class Hotels
{
    public record GetAll_Data(
        int hotel_id,
        int city_id,
        string name,
        int price_per_night,
        int available_rooms,
        int? rating,
        bool has_wifi
    );

    // GET Hotels → kan filtrera på land, stad eller sökterm
    public static async Task<List<GetAll_Data>> Get(
        int? country_id,
        int? city_id,
        string? searchTerm,
        Config config
    )
    {
        List<GetAll_Data> result = new();

        // Bas-query med JOIN för att kunna filtrera på land
        string query = """
            SELECT h.hotel_id, h.city_id, h.name,
                   h.price_per_night, h.available_rooms,
                   h.rating, h.has_wifi
            FROM hotels h
            JOIN cities c ON h.city_id = c.city_id
            """;

        List<string> filters = new();
        List<MySqlParameter> parameters = new();

        // Filtrera på land
        if (country_id.HasValue)
        {
            filters.Add("c.country_id = @country_id");
            parameters.Add(new("@country_id", country_id.Value));
        }

        // Filtrera på stad
        if (city_id.HasValue)
        {
            filters.Add("h.city_id = @city_id");
            parameters.Add(new("@city_id", city_id.Value));
        }

        // Filtrera på namn-sökning
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            filters.Add("h.name LIKE @search");
            parameters.Add(new("@search", "%" + searchTerm + "%"));
        }

        // Lägg till WHERE om filter finns
        if (filters.Count > 0)
        {
            query += " WHERE " + string.Join(" AND ", filters);
        }

        using var reader =
            await MySqlHelper.ExecuteReaderAsync(config.db, query, parameters.ToArray());

        while (reader.Read())
        {
            result.Add(new(
                reader.GetInt32(0),               // hotel_id
                reader.GetInt32(1),               // city_id
                reader.GetString(2),              // name
                reader.GetInt32(3),               // price_per_night
                reader.GetInt32(4),               // available_rooms
                reader.IsDBNull(5) ? null : reader.GetInt32(5), // rating
                reader.GetInt32(6) == 1           // has_wifi → bool
            ));
        }

        return result;
    }
}