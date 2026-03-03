using Cassandra;
using Microsoft.Extensions.Configuration;

namespace HoloRed.Infrastructure.Cassandra
{
    public class CassandraSessionFactory
    {
        private readonly CassandraSettings _settings;
        private readonly Lazy<ISession> _session;

        public CassandraSessionFactory(IConfiguration configuration)
        {
            _settings = configuration.GetSection("Cassandra").Get<CassandraSettings>() ?? new CassandraSettings();

            _session = new Lazy<ISession>(() =>
            {
                var cluster = Cluster.Builder()
                    .AddContactPoint(_settings.Host)
                    .WithPort(_settings.Port)
                    .Build();

                var session = cluster.Connect(_settings.Keyspace);
                return session;
            });
        }

        public ISession GetSession() => _session.Value;
    }
}