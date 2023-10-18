using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace DemoDataProviderFactory
{
    public class Program
    {

        static string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var strConnection = config["ConnectionStrings:MyStoreDB"];
            return strConnection;
        }

        static void ViewProduct()
        {
            DbProviderFactory factory = SqlClientFactory.Instance;
            using DbConnection connection = factory.CreateConnection();
            if (connection == null)
            {
                Console.WriteLine($"Unable to create the connection object");
                return;
            }
            connection.ConnectionString = GetConnectionString();
            connection.Open();
            DbCommand command = connection.CreateCommand();
            if (command == null)
            {
                Console.WriteLine($"Unable to create the connection object");
                return;
            }
            command.Connection = connection;
            command.CommandText = "Select * from Product";
            using DbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"ProductID: {reader["ProductID"]} - Product Name: {reader["ProductName"]}");
            }
        }
        public static void Main(string[] args)
        {
            ViewProduct();
        }
    }
}