using HoloRed.Domain.Exceptions;
using HoloRed.Domain.Inteligencia;
using Neo4j.Driver;

namespace HoloRed.Infrastructure.Neo4j
{
    public class Neo4jInteligenciaRepository : IInteligenciaRepository
    {
        private readonly IDriver _driver;

        public Neo4jInteligenciaRepository(Neo4jDriverFactory factory)
        {
            _driver = factory.GetDriver();
        }

        public async Task<IReadOnlyList<TraidorDto>> GetTraidoresAsync(string faccion)
        {
            const string cypher = @"
                MATCH (e:Espia)-[:INFILTRADO_EN]->(f:Faccion {nombre: $faccion})
                MATCH (e)-[:SUMINISTRA_ARMAS_A]->(r:Faccion)
                WHERE r.nombre <> $faccion
                RETURN e.nombre AS espia, r.nombre AS rival
            ";

            try
            {
                await using var session = _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Read));

                var cursor = await session.RunAsync(cypher, new { faccion });
                var records = await cursor.ToListAsync();

                var result = records.Select(r => new TraidorDto
                {
                    Espia = r["espia"].As<string>(),
                    FaccionRival = r["rival"].As<string>()
                }).ToList();

                return result;
            }
            catch (ServiceUnavailableException ex)
            {
                throw new ExternalServiceUnavailableException("NEO4J", "Neo4j no disponible", "NEO4J_UNAVAILABLE", ex);
            }
            catch (SessionExpiredException ex)
            {
                throw new ExternalServiceUnavailableException("NEO4J", "Sesión Neo4j expirada", "NEO4J_SESSION_EXPIRED", ex);
            }
            catch (AuthenticationException ex)
            {
                throw new ExternalServiceUnavailableException("NEO4J", "Credenciales Neo4j inválidas", "NEO4J_AUTH_FAILED", ex);
            }
        }
    }
}