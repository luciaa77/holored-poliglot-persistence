using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace HoloRed.Infrastructure.Redis
{
    public class RedisConnection
    {
        private readonly ConnectionMultiplexer _connection;

        public IDatabase Database => _connection.GetDatabase();

        public RedisConnection(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            var password = configuration["REDIS_PASSWORD"];
            var options = ConfigurationOptions.Parse("localhost:6379");
            options.Password = password;
            options.AbortOnConnectFail = false;

            _connection = ConnectionMultiplexer.Connect(options);
        }
    }
}