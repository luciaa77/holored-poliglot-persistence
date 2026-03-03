using System.Collections.Generic;
using System.Threading.Tasks;

namespace HoloRed.Domain.Inteligencia
{
    public interface IInteligenciaRepository
    {
        Task<IReadOnlyList<TraidorDto>> GetTraidoresAsync(string faccion);
    }
}