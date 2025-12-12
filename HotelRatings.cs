namespace server;

using MySql.Data.MySqlClient;

class HotelRatings
{
    //data that returns when we fetch ratings
    public record GetAll_Data(int Rating_Id, int User_id, int Hotel_Id, int Score, string? Comment, DateTime Created_At);

    public static async Task<List<GetAll_Data>> GetByHotel(int hotelId, Config config)     //GET hotels/hotel_id/ratings
    {
        List<GetAll_Data> result = new();

        string query = """
            SELECT rating_id, user_id, hotel_id, score, comment, created_at
            FROM hotel_ratings
            WHERE hotel_id = @hotel_id
            ORDER BY created_at DESC
        """;

        var parameters = new MySqlParameter[]
        {
            new("@hotel_id", hotelId)
        };

        using (var reader = await MySqlHelper.ExecuteReaderAsync(config.db, query, parameters))
        {
            while (reader.Read())
            {
                result.Add(new(
                    reader.GetInt32(0),               //rating id 
                    reader.GetInt32(1),              //user id
                    reader.GetInt32(2),             // hotel id
                    reader.GetInt32(3),            //score
                    reader.IsDBNull(4) ? null : reader.GetString(4),
                    reader.GetDateTime(5)   //created at 
                ));
            }
        }
        return result;
    }

    public record Post_Args(int User_Id, int Score, string? Comment);
    public static async Task Post(int hotelId, Post_Args rating, Config config)
    {
        string query = """
            INSERT INTO hotel_ratings(user_id, hotel_id, score, comment)
            VALUES (@user_id, @hotel_id, @score, @comment)
        """;

        var parameters = new MySqlParameter[]
        {
            new("@user_id", rating.User_Id),
            new("@hotel_id", hotelId),
            new("@score", rating.Score),
            new("@comment", rating.Comment ?? (object)DBNull.Value),
        };

        await MySqlHelper.ExecuteNonQueryAsync(config.db, query, parameters);
    }


}