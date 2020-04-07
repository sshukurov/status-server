using Npgsql;

namespace Tedu.Server.Status.Web.Configuration
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string DatabaseServer { get; set; }
        public ushort DatabasePort { get; set; }

        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string GetConnectionString()
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder
            {
                Host = DatabaseServer,
                Port = DatabasePort,
                Database = DatabaseName,
                Username = UserName,
                Password = Password
            };
            return connectionStringBuilder.ToString();
        }
    }
}