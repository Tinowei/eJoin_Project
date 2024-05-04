using Microsoft.Data.SqlClient;

namespace Admin.Helpers;

public class ConnectDBHelper
{
    private static ConnectDBHelper _instance;
    private readonly string _connectionString;

    public ConnectDBHelper(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("EJoinDB");
    }

    public static ConnectDBHelper GetInstance(IConfiguration configuration)
    {
        if (_instance == null)
        {
            _instance = new ConnectDBHelper(configuration);
        }
        return _instance;
    }
    
    public SqlConnection OpenConnection()
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}