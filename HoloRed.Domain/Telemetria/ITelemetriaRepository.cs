using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HoloRed.Domain.Telemetria
{
    public interface ITelemetriaRepository
    {
        Task InsertImpactoAsync(Impacto impacto);
        Task<IReadOnlyList<Impacto>> GetHistorialAsync(string sectorId, DateOnly fecha);
    }
}