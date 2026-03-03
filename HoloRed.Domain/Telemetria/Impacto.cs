namespace HoloRed.Domain.Telemetria
{
    public class Impacto
    {
        public string SectorId { get; set; } = string.Empty;
        public DateOnly Fecha { get; set; }          // día (YYYY-MM-DD)
        public DateTime Timestamp { get; set; }      // momento exacto del impacto

        public string NaveAtacante { get; set; } = string.Empty;
        public string NaveObjetivo { get; set; } = string.Empty;

        public int DanoEscudos { get; set; }
    }
}