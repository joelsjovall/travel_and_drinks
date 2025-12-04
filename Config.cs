public class Config
{
    public string ConnectionString { get; }
    public Config(string connectionString) => ConnectionString = connectionString;
}