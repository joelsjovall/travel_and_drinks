namespace server;

static class Profile
{
    public record Get_Data(string Email, string? Name);
    public static async Task<Get_Data?>
    Get(Config config, HttpContext ctx)
    {
        Get_Data? result = null;

        if (ctx.Session.IsAvailable)
        {
            if (ctx.Session.GetInt32("user_id") is int user_id)
            {

                string query = "SELECT email FROM users WHERE id = @id";
                var parameters = new MySqlParameter[] { new("@id", user_id) };
                using (var reader = await MySqlHelper.ExecuteReaderAsync(config.db, query, parameters))
                {
                    if (reader.Read())
                    {
                        object name_or_null = reader[1];
                        string email = reader.GetString(0);
                        string? name = null;

                        if (reader[1] is string s)
                        {
                            name = s;
                        }
                        result = new(email, name);
                    }
                }
            }
        }

        return result;
    }
}