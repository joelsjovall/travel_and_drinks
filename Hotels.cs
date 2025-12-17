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
        bool has_wifi,
        bool has_pool
    );

    public record CreateHotel_Data(
    int city_id,
    string name,
    int price_per_night,
    int available_rooms,
    int? rating,
    bool has_wifi,
    bool has_pool
);

    public record UpdateHotel_Data(
      string? name,
      int? price_per_night,
      int? available_rooms,
      int? rating,
      bool? has_wifi,
      bool? has_pool
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
                   h.rating, h.has_wifi, h.has_pool
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
                reader.GetInt32(6) == 1,           // has_wifi → bool
                reader.GetInt32(7) == 1           // has_pool

            ));
        }

        return result;
    }

    public static async Task<int> Create(CreateHotel_Data data, Config config)
    {
        string query = """
            INSERT INTO hotels (city_id, name, price_per_night, available_rooms, rating, has_wifi, has_pool)
            VALUES (@city_id, @name, @price, @rooms, @rating, @wifi, @pool);
            SELECT LAST_INSERT_ID();
        """;

        var parameters = new MySqlParameter[]
        {
            new("@city_id", data.city_id),
            new("@name", data.name),
            new("@price", data.price_per_night),
            new("@rooms", data.available_rooms),
            new("@rating", data.rating),
            new("@wifi", data.has_wifi ? 1 : 0),
            new("@pool", data.has_pool ? 1 : 0)
        };

        object result = await MySqlHelper.ExecuteScalarAsync(config.db, query, parameters);
        return Convert.ToInt32(result);
    }


    //uppdatera hotell


    public static async Task<bool> Update(int id, UpdateHotel_Data data, Config config)
    {
        string query = """
            UPDATE hotels SET
                name = COALESCE(@name, name),
                price_per_night = COALESCE(@price, price_per_night),
                available_rooms = COALESCE(@rooms, available_rooms),
                rating = COALESCE(@rating, rating),
                has_wifi = COALESCE(@wifi, has_wifi),
                has_pool = COALESCE(@pool, has_pool)

            WHERE hotel_id = @id;
        """;

        var parameters = new MySqlParameter[]
        {
            new("@id", id),
            new("@name", data.name),
            new("@price", data.price_per_night),
            new("@rooms", data.available_rooms),
            new("@rating", data.rating),
            new("@wifi", data.has_wifi.HasValue ? (data.has_wifi.Value ? 1 : 0) : null),
            new("@pool", data.has_pool.HasValue ? (data.has_pool.Value ? 1 : 0) : null)
        };

        int rows = await MySqlHelper.ExecuteNonQueryAsync(config.db, query, parameters);
        return rows > 0;
    }


    // DELETE / ta bort hotell
    public static async Task<bool> Delete(int id, Config config)
    {   

           
        string query = "DELETE FROM hotels WHERE hotel_id = @id";

        var parameters = new MySqlParameter[]
        {
            new("@id", id)
        };

        int rows = await MySqlHelper.ExecuteNonQueryAsync(config.db, query, parameters);
        return rows > 0;
    }
}



// Hämta alla hotell
// GET http://localhost:5000/hotels


// Hämta hotell med filter 
// GET http://localhost:5000/hotels?city_id=1
// GET http://localhost:5000/hotels?country_id=2
// GET http://localhost:5000/hotels?searchTerm=Grand


// Skapa nytt hotell
// POST http://localhost:5000/hotels
// Body : (ett exempel)
// {
//   "city_id": 1,
//   "name": "Test Hotel",
//   "price_per_night": 200,
//   "available_rooms": 10,
//   "rating": 4,
//   "has_wifi": true,
//   "has_pool": false
// }


// Uppdatera hotell (t.ex. pool / wifi / pris)
// PUT http://localhost:5000/hotels/{id}
// Exempel:
// PUT http://localhost:5000/hotels/2
// Body:
// {
//   "has_pool": true eller false
// }
// Body:
// {
//   "has_wifi": true eller false
// }

// Ta bort hotell
// DELETE http://localhost:5000/hotels/{id}
// Exempel:
// DELETE http://localhost:5000/hotels/5