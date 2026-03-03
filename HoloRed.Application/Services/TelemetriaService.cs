using HoloRed.Domain.Telemetria;

namespace HoloRed.Application.Services
{
    public class TelemetriaService
    {
        private readonly ITelemetriaRepository _repo;

        public TelemetriaService(ITelemetriaRepository repo)
        {
            _repo = repo;
        }

        public Task InsertImpactoAsync(Impacto impacto)
        {
            // Validaciones básicas (400 si viniera mal, lo gestionará el controller)
            if (string.IsNullOrWhiteSpace(impacto.SectorId))
                throw new ArgumentException("SectorId es obligatorio");

            if (string.IsNullOrWhiteSpace(impacto.NaveAtacante))
                throw new ArgumentException("NaveAtacante es obligatoria");

            if (string.IsNullOrWhiteSpace(impacto.NaveObjetivo))
                throw new ArgumentException("NaveObjetivo es obligatoria");

            if (impacto.DanoEscudos < 0)
                throw new ArgumentException("DanoEscudos no puede ser negativo");

            return _repo.InsertImpactoAsync(impacto);
        }

        public Task<IReadOnlyList<Impacto>> GetHistorialAsync(string sectorId, DateOnly fecha)
        {
            if (string.IsNullOrWhiteSpace(sectorId))
                throw new ArgumentException("SectorId es obligatorio");

            return _repo.GetHistorialAsync(sectorId, fecha);
        }
    }
}