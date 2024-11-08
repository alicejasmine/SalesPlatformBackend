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
        $"Server=localhost,1433;Database=SalesPlatformDB;User Id = sa; Password=Suits0811;TrustServerCertificate=True;";
    
    public static string GetConnectionString()
    {
        string connectionString = Environment.GetEnvironmentVariable("sqlconn");
        return connectionString ?? BuildConnectionString();
    }
}