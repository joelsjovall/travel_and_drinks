namespace server;

using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Prng;

class Events
{
    public record GetAll_Data(int event_id, string name, string drink_type, DateTime event_date, int price_per_person, int available_seats);
    public static async Task<List<GetAll_Data>>

    GetAll(Config config)
    {
        List<GetAll_Data> result = new();

        string query = """SELECT event_id, name, drink_type, event_date, price_per_person, available_seats FROM events ORDER BY event_date""";

        using (var reader = await MySqlHelper.ExecuteReaderAsync(config.db, query))
        {
            while (reader.Read())
            {
                result.Add(new(
                    reader.GetInt32(0),  //reads column 0 (event_id) as int
                    reader.GetString(1),  //reads column 1 (name) as string
                    reader.GetString(2),  //reads column 2 (drink_type) as string
                    reader.GetDateTime(3),  //reads column 3 (event_date) as datetime
                    reader.GetInt32(4),  //reads column 4 (price_per_person) as int
                    reader.GetInt32(5)  //reads column 5 (available_seats) as int
                ));
            }
        }

        return result;
    }

    public static async Task<List<GetAll_Data>> Search(string? search, Config config)   // search method. ex: localhost:5000/events?search=beer
    {
        if (string.IsNullOrEmpty(search))
            return await GetAll(config);

        List<GetAll_Data> result = new();

        string query = """SELECT event_id, name, drink_type, event_date, price_per_person, available_seats FROM events WHERE name LIKE @search OR drink_type LIKE @search""";

        var parameters = new MySqlParameter[]
        {
            new("@search", "%" + search + "%")
        };

        using (var reader = await MySqlHelper.ExecuteReaderAsync(config.db, query, parameters))
        {
            while (reader.Read())
            {
                result.Add(new(
                    reader.GetInt32(0),  //reads column 0 (event_id) as int
                    reader.GetString(1),  //reads column 1 (name) as string
                    reader.GetString(2),  //reads column 2 (drink_type) as string
                    reader.GetDateTime(3),  //reads column 3 (event_date) as datetime
                    reader.GetInt32(4),  //reads column 4 (price_per_person) as int
                    reader.GetInt32(5)  //reads column 5 (available_seats) as int
                ));
            }
        }

        return result;
    }

    // For GET(id)
    public record City_Data(int city_id, string city_name, int country_id, string country_name);
    public record Hotel_Data(int hotel_id, string hotel_name);

    public record Get_Data(
        int event_id,
        string name,
        string drink_type,
        DateTime event_date,
        int price_per_person,
        int available_seats,
        City_Data City,
        List<Hotel_Data> NearbyHotels
    );

    public static async Task<Get_Data?> Get(int id, Config config)  //GET one event by id ex: http://localhost:5000/events/5
    {
        Get_Data? result = null;

        string query = """ SELECT e.event_id, e.name, e.drink_type, e.event_date, e.price_per_person, e.available_seats, c.city_id, c.name, co.country_id, co.name, h.hotel_id, h.name FROM events e JOIN cities c ON c.city_id = e.city_id JOIN countries co ON co.country_id = c.country_id LEFT JOIN hotels h ON h.city_id = c.city_id WHERE e.event_id = @id""";

        var parameters = new MySqlParameter[] { new("@id", id) };

        using (var reader = await MySqlHelper.ExecuteReaderAsync(config.db, query, parameters))
        {
            List<Hotel_Data> hotels = new();
            int eventId = 0;
            string eventName = "";
            string drinkType = "";
            DateTime eventDate = DateTime.Now;
            int price = 0;
            int seats = 0;

            City_Data? cityData = null;

            while (reader.Read())
            {
                if (result is null)
                {
                    eventId = reader.GetInt32(0);
                    eventName = reader.GetString(1);
                    drinkType = reader.GetString(2);
                    eventDate = reader.GetDateTime(3);
                    price = reader.GetInt32(4);
                    seats = reader.GetInt32(5);

                    cityData = new City_Data(
                        reader.GetInt32(6),
                        reader.GetString(7),
                        reader.GetInt32(8),
                        reader.GetString(9)
                    );
                }

                if (!reader.IsDBNull(10))
                {
                    hotels.Add(new Hotel_Data(
                        reader.GetInt32(10),
                        reader.GetString(11)
                    ));
                }
                result = new (eventId, eventName, drinkType, eventDate, price, seats, cityData!, hotels);
            }
        }
        return result;
    }

    /*POST  ex : http://localhost:5000/events body: {
    "city_id": 1,
    "name": "Beer Tasting Night",
    "drink_type": "Beer",
    "event_date": "2025-03-01T19:00:00",
    "price_per_person": 150,
    "available_seats": 50
    }*/
    public record Post_Args(
        int city_id,
        string name, 
        string drink_type,
        DateTime event_date,
        int price_per_person,
        int available_seats,
        int user_id
    );

    public static async Task Post(Post_Args e, Config config)
    {
        string query = """ INSERT INTO events(city_id, name, drink_type, event_date, price_per_person, available_seats) SELECT @city_id, @name, @drink, @date, @price, @seats FROM users WHERE user_id = @user_id AND admin = 1 """;

        var parameters = new MySqlParameter[]
        {
            new("@city_id", e.city_id),
            new("@name", e.name),
            new("@drink", e.drink_type),
            new("@date", e.event_date),
            new("@price", e.price_per_person),
            new("@seats", e.available_seats),
            new("user_id", e.user_id)
        };

int row = await MySqlHelper.ExecuteNonQueryAsync(config.db, query, parameters);
 if(row == 0)
        {
             throw new UnauthorizedAccessException("Endast admin får lägga upp event.");


        }
     
    }

    /*PUT ex: http://localhost:5000/events/3 body: {
    "city_id": 1,
    "name": "Updated Beer Tasting Night",
    "drink_type": "Beer",
    "event_date": "2025-04-12T20:00:00",
    "price_per_person": 180,
    "available_seats": 45,
    "user_id: 1
    }*/
    public record Put_Args(
        int city_id,
        string name,
        string drink_type,
        DateTime event_date,
        int price_per_person,
        int available_seats,
        int user_id
    );

    public static async Task Put(int id, Put_Args e, Config config)
    {
        string query = """ 
        UPDATE events e
        JOIN users u ON u.user_id = @user_id
        SET
            e.city_id = @city_id,
            e.name = @name,
            e.drink_type = @drink,
            e.event_date = @date,
            e.price_per_person = @price,
            e.available_seats = @seats
        WHERE
            e.event_id = @id
            AND u.admin = 1
 """;

        var parameters = new MySqlParameter[]
        {
            new("@id", id),
            new("@city_id", e.city_id),
            new("@name", e.name),
            new("@drink", e.drink_type),
            new("@date", e.event_date),
            new("@price", e.price_per_person),
            new("@seats", e.available_seats),
            new("@user_id", e.user_id)
        };
int row = await MySqlHelper.ExecuteNonQueryAsync(config.db, query, parameters);
 if(row == 0)
        {
             throw new UnauthorizedAccessException("Endast admin får lägga upp event.");


        }
    }

    //DELETE
    // http://localhost:5000/events/22?user_id=3
    public static async Task Delete(int id,int user_id, Config config)
    {
        string query = """
        DELETE e
        FROM events e
        JOIN users u ON u.user_id = @user_id
        WHERE e.event_id = @id
          AND u.admin = 1
    """;

        var parameters = new MySqlParameter[]
        {
            new("@id", id),
            new("@user_id", user_id)
        };

        await MySqlHelper.ExecuteNonQueryAsync(config.db, query,parameters);
    }

}