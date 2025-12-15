namespace server;

using MySql.Data.MySqlClient;

class EventRatings
{
    public record GetAll_Data(int Rating_Id, int User_Id, int Event_Id, int Score, string? Comment, DateTime Created_At);

    public static async Task<List<GetAll_Data>> GetByEvent(int eventId, Config config)
    {
        List<GetAll_Data> result = new();

        string Query = """
            SELECT rating_id, user_id, event_id, score, comment, created_at
            FROM event_ratings
            WHERE event_id = @event_id
            ORDER BY created_at DESC
        """;

        var parameters = new MySqlParameter[]
        {
            new("@event_id", eventId)
        };

        using (var reader = await MySqlHelper.ExecuteReaderAsync(config.db, Query, parameters))
        {
            while (reader.Read())
            {
                result.Add(new(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.IsDBNull(4) ? null : reader.GetString(4),
                    reader.GetDateTime(5)
                ));
            }
        }

        return result;
    }

    public record Post_Args(int User_Id, int Score, string? Comment);
    public static async Task Post(int eventId, Post_Args rating, Config config)
    {
        string query = """
            INSERT INTO event_ratings(user_id, event_id, score, comment)
            VALUES (@user_id, @event_id, @score, @comment)
        """;

        var parameters = new MySqlParameter[]
        {
            new("@user_id", rating.User_Id),
        new("@event_id", eventId),
            new("@score", rating.Score),
            new("@comment", rating.Comment ?? (object)DBNull.Value),
        };

        await MySqlHelper.ExecuteNonQueryAsync(config.db, query, parameters);
    }


}