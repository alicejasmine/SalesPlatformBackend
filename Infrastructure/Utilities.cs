namespace Infrastructure;

public class Utilities
{
    private const string DefaultServer = "localhost";
    private const int DefaultPort = 1433;
   

    public static string BuildConnectionString(
        string server = DefaultServer,
        string database = "",
        string user = "",
        string password = "",
        bool trustServerCertificate = true) =>
        $"Server={server},{DefaultPort};Database={database};User Id={user};Password={password};TrustServerCertificate={trustServerCertificate.ToString().ToUpper()}";
    
    public static string GetConnectionString()
    {
        string connectionString = Environment.GetEnvironmentVariable("sqlconn");
        return connectionString ?? BuildConnectionString();
    }
}