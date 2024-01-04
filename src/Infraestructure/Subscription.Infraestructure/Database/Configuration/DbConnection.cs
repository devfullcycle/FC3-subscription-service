namespace Subscription.Infraestructure.Database.Configuration
{
    public class DbConnection
    {
        public string Server { get; set; }
        public string Port { get; set; }
        public string Database { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string GetConnectionString() =>
            $"Host={Server};Port={Port};Database={Database};Username={UserName};Password={Password};";
    }
}
