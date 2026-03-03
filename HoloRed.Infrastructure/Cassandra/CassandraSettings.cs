namespace HoloRed.Infrastructure.Cassandra
{
    public class CassandraSettings
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 9042;
        public string Keyspace { get; set; } = "holored";
    }
}