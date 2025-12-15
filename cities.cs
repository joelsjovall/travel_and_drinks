namespace server;

using System.Runtime.CompilerServices;
using MySql.Data.MySqlClient;
public record CitySearchRequest(string? City);


class Cities
{
 public record GetAll_Data(int city_id, int country_id, string name,string country_name,string eventName,string hotelname);

    public static async Task<List<GetAll_Data>> 
    Get(string? searchTerm, Config config)
    {
        List<GetAll_Data> result = new();

        string query;
        MySqlParameter[] parameters;

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            // Om sökterm finns använd LIKE
            query =   """
        SELECT c.city_id, c.country_id, c.name AS city_name, 
        co.name AS country_name,
         e.name AS event_name,
          h.name AS hotel_name
        FROM d_a_t.cities c
        JOIN d_a_t.countries co ON c.country_id = co.country_id
        LEFT JOIN d_a_t.events e ON e.city_id = c.city_id
        LEFT JOIN d_a_t.hotels h ON h.city_id = c.city_id
        WHERE c.name LIKE @search
        """;
            parameters = new MySqlParameter[] { new("@search", "%" + searchTerm + "%") };
        }
        else
        {
            // Ingen sökterm hämta alla
            query = """
        SELECT c.city_id, c.country_id, c.name AS city_name, 
               co.name AS country_name,
               e.name AS event_name,
                h.name AS hotel_name
        FROM d_a_t.cities c
        JOIN d_a_t.countries co ON c.country_id = co.country_id
        LEFT JOIN d_a_t.events e ON e.city_id = c.city_id
         LEFT JOIN d_a_t.hotels h ON h.city_id = c.city_id
        """;
            parameters = Array.Empty<MySqlParameter>();
        }

        using (var reader = await MySqlHelper.ExecuteReaderAsync(config.db, query, parameters))
        {
            while (reader.Read())
            {
                result.Add(new(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetString(2),
                    reader.GetString(3),
                     reader.IsDBNull(4) ? "There is not event going on here right now" : reader.GetString(4), // hantera null
                     reader.IsDBNull(5)?"There is not hotel in this city yet": reader.GetString(5)
                ));
            }
        }
        return result;
    }

    
   public record Post_Args(string Name,int country_id, string user_email);
   public static async Task
 Post(Post_Args city, Config config)
{         

    
    string query = """
     
        INSERT INTO cities(name, country_id)
        SELECT @name , @country_id
        FROM
         users where email = @user_email
        AND admin = true
        """;

    var parameters = new MySqlParameter[]
    {
        new("@user_email", city.user_email),   
        new("@name", city.Name),
        new("@country_id", city.country_id)
    };
 await MySqlHelper.ExecuteNonQueryAsync(config.db, query, parameters);

}
/*
  
  "user_email": "kebaiburk",
   "name": "Falafeltown",
   "country_id": 3
  */
 



    public record Put_Args(int countryid, string Name,int cityid);
        public static async Task
        Put(Put_Args City, Config config)
        {
            string query = """
                UPDATE cities
                SET country_id = @country_id, name = @name
                WHERE city_id = @cityid
                """;

            var parameters = new MySqlParameter[]
            {
                new("@country_id", City.countryid),
                    new("@cityid", City.cityid),
                    new("@name", City.Name),
            };

            await MySqlHelper.ExecuteNonQueryAsync(config.db, query, parameters);
        }
}

