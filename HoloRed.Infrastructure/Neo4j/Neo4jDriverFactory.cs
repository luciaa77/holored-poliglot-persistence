using Microsoft.Extensions.Configuration;
using Neo4j.Driver;

namespace HoloRed.Infrastructure.Neo4j
{
    public class Neo4jDriverFactory
    {
        private readonly Neo4jSettings _settings;
        private readonly Lazy<IDriver> _driver;

        public Neo4jDriverFactory(IConfiguration configuration)
        {
            _settings = configuration.GetSection("Neo4j").Get<Neo4jSettings>() ?? new Neo4jSettings();

            _driver = new Lazy<IDriver>(() =>
                GraphDatabase.Driver(
                    _settings.Uri,
                    AuthTokens.Basic(_settings.User, _settings.Password)
                )
            );
        }

        public IDriver GetDriver() => _driver.Value;
    }
}