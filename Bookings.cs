namespace server;

using MySql.Data.MySqlClient;


class Bookings
{
    public record Joined_Data(
        int booking_id,
        int user_id,
        string user_email,
        int? hotel_id,
        string? hotel_name,
        int? event_id,
        string? event_name,
        int total_price
    );

    private static Joined_Data ReadJoined(MySqlDataReader r)     //read joined rows
    {
        return new Joined_Data(
            r.GetInt32(0),
            r.GetInt32(1),
            r.GetString(2),
            r.IsDBNull(3) ? null : r.GetInt32(3),
            r.IsDBNull(4) ? null : r.GetString(4),
            r.IsDBNull(5) ? null : r.GetInt32(5),
            r.IsDBNull(6) ? null : r.GetString(6),
            r.GetInt32(7)
        );
    }

    public static async Task<List<Joined_Data>> Search(string? search, Config config)
    //search for user email, hotel name or event name. http://localhost:5000/bookings/?search=månsoskarsson@hotmail.com
    {
        if (string.IsNullOrEmpty(search))
            return await GetAllJoined(config);

        List<Joined_Data> result = new();

        string query = """ SELECT b.booking_id, u.user_id, u.email, h.hotel_id, h.name, e.event_id, e.name, b.total_price FROM bookings b JOIN users u ON u.user_id = b.user_id LEFT JOIN hotels h ON h.hotel_id = b.hotel_id LEFT JOIN events e ON e.event_id = b.event_id WHERE u.email LIKE @search OR h.name LIKE @search OR e.name LIKE @search """;

        var p = new MySqlParameter[]
        {
            new("@search", "%" + search + "%")
        };

        using var reader = await MySqlHelper.ExecuteReaderAsync(config.db, query, p);

        while (reader.Read())
        {
            result.Add(ReadJoined(reader));
        }

        return result;
    }

    public static async Task<List<Joined_Data>> GetAllJoined(Config config)    //get full list with users, hotel and events included , http://localhost:5000/bookings
    {
        List<Joined_Data> result = new();

        string query = """ SELECT b.booking_id, u.user_id, u.email, h.hotel_id, h.name, e.event_id, e.name, b.total_price FROM bookings b JOIN users u ON u.user_id = b.user_id LEFT JOIN hotels h ON h.hotel_id = b.hotel_id LEFT JOIN events e ON e.event_id = b.event_id """;

        using var reader = await MySqlHelper.ExecuteReaderAsync(config.db, query);

        while (reader.Read())
        {
            result.Add(ReadJoined(reader));
        }

        return result;
    }

    public record Post_Args(
        int user_id,
        int? hotel_id,
        int? event_id,
        int total_price
    );

    public static async Task Post(Post_Args booking, Config config)
    /*POST ex: POST http://localhost:5000/bookings and 
    {
    "user_id": 1,
    "hotel_id": 2,
    "event_id": 3,
    "total_price": 200
    }
    */
    {
        string query = """ INSERT INTO bookings (user_id, hotel_id, event_id, total_price) VALUES (@user_id, @hotel_id, @event_id, @total_price) """;

        var p = new MySqlParameter[]
        {
            new("@user_id", booking.user_id),
            new("@hotel_id", booking.hotel_id),
            new("@event_id", booking.event_id),
            new("@total_price", booking.total_price)
        };

        await MySqlHelper.ExecuteNonQueryAsync(config.db, query, p);
    }

    public record Put_Args(
        int user_id,
        int? hotel_id,
        int? event_id,
        int total_price
    );

    public static async Task Put(int id, Put_Args booking, Config config)
    /*PUT http://localhost:5000/bookings/5 ex: 
    {
    "user_id": 1,
    "hotel_id": null,
    "event_id": 7,
    "total_price": 350
    }
    */
    {
        string query = """ UPDATE bookings SET user_id = @user_id, hotel_id = @hotel_id, event_id = @event_id, total_price = @total_price WHERE booking_id = @id """;

        var p = new MySqlParameter[]
        {
            new("@id", id),
            new("@user_id", booking.user_id),
            new("@hotel_id", booking.hotel_id),
            new("@event_id", booking.event_id),
            new("@total_price", booking.total_price)
        };

        await MySqlHelper.ExecuteNonQueryAsync(config.db, query, p);
    }

    public static async Task Delete(int id, Config config)  //DELETE http://localhost:5000/bookings/5
    {
        string query = "DELETE FROM bookings WHERE booking_id = @id";

        var p = new MySqlParameter[]
        {
            new("@id", id)
        };
        await MySqlHelper.ExecuteNonQueryAsync(config.db, query, p);
    }

    // Get all bookings for a user
    public static async Task<List<Joined_Data>> GetByUser(int userId, Config config)
    {
        List<Joined_Data> result = new();

        string query = """
        SELECT b.booking_id, u.user_id, u.email,
               h.hotel_id, h.name,
               e.event_id, e.name,
               b.total_price
        FROM bookings b
        JOIN users u ON u.user_id = b.user_id
        LEFT JOIN hotels h ON h.hotel_id = b.hotel_id
        LEFT JOIN events e ON e.event_id = b.event_id
        WHERE b.user_id = @userId
    """;

        var p = new MySqlParameter[]
        {
        new("@userId", userId)
        };

        using var reader = await MySqlHelper.ExecuteReaderAsync(config.db, query, p);

        while (reader.Read())
        {
            result.Add(ReadJoined(reader));
        }

        return result;
    }

}