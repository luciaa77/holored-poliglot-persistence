using Cassandra;
using HoloRed.Domain.Exceptions;
using HoloRed.Domain.Telemetria;

namespace HoloRed.Infrastructure.Cassandra
{
    public class CassandraTelemetriaRepository : ITelemetriaRepository
    {
        private readonly ISession _session;
        private readonly PreparedStatement _insertStmt;
        private readonly PreparedStatement _selectStmt;

        public CassandraTelemetriaRepository(CassandraSessionFactory factory)
        {
            _session = factory.GetSession();

            // IMPORTANTE: tabla impactos debe existir en keyspace holored
            _insertStmt = _session.Prepare(@"
                INSERT INTO impactos (sector_id, fecha, ts, nave_atacante, nave_objetivo, dano_escudos)
                VALUES (?, ?, ?, ?, ?, ?);");

            _selectStmt = _session.Prepare(@"
                SELECT sector_id, fecha, ts, nave_atacante, nave_objetivo, dano_escudos
                FROM impactos
                WHERE sector_id = ? AND fecha = ?;");
        }

        public async Task InsertImpactoAsync(Impacto impacto)
        {
            try
            {
                // Cassandra "date" => LocalDate (NO DateTime)
                var cassFecha = new LocalDate(impacto.Fecha.Year, impacto.Fecha.Month, impacto.Fecha.Day);

                var bound = _insertStmt.Bind(
                    impacto.SectorId,
                    cassFecha,
                    impacto.Timestamp,
                    impacto.NaveAtacante,
                    impacto.NaveObjetivo,
                    impacto.DanoEscudos
                );

                await _session.ExecuteAsync(bound).ConfigureAwait(false);
            }
            catch (NoHostAvailableException ex)
            {
                throw new ExternalServiceUnavailableException("CASSANDRA", "Cassandra no disponible", "CASSANDRA_UNAVAILABLE", ex);
            }
            catch (OperationTimedOutException ex)
            {
                throw new ExternalServiceUnavailableException("CASSANDRA", "Timeout conectando con Cassandra", "CASSANDRA_TIMEOUT", ex);
            }
        }

        public async Task<IReadOnlyList<Impacto>> GetHistorialAsync(string sectorId, DateOnly fecha)
        {
            try
            {
                // Cassandra "date" => LocalDate
                var cassFecha = new LocalDate(fecha.Year, fecha.Month, fecha.Day);

                var bound = _selectStmt.Bind(
                    sectorId,
                    cassFecha
                );

                var rs = await _session.ExecuteAsync(bound).ConfigureAwait(false);

                var list = new List<Impacto>();
                foreach (var row in rs)
                {
                    var rowFecha = row.GetValue<LocalDate>("fecha");

                    list.Add(new Impacto
                    {
                        SectorId = row.GetValue<string>("sector_id"),
                        Fecha = new DateOnly(rowFecha.Year, rowFecha.Month, rowFecha.Day),
                        Timestamp = row.GetValue<DateTime>("ts"),
                        NaveAtacante = row.GetValue<string>("nave_atacante"),
                        NaveObjetivo = row.GetValue<string>("nave_objetivo"),
                        DanoEscudos = row.GetValue<int>("dano_escudos")
                    });
                }

                return list;
            }
            catch (NoHostAvailableException ex)
            {
                throw new ExternalServiceUnavailableException("CASSANDRA", "Cassandra no disponible", "CASSANDRA_UNAVAILABLE", ex);
            }
            catch (OperationTimedOutException ex)
            {
                throw new ExternalServiceUnavailableException("CASSANDRA", "Timeout conectando con Cassandra", "CASSANDRA_TIMEOUT", ex);
            }
        }
    }
}