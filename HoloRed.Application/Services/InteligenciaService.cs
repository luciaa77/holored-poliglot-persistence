using HoloRed.Domain.Inteligencia;

namespace HoloRed.Application.Services
{
    public class InteligenciaService
    {
        private readonly IInteligenciaRepository _repo;

        public InteligenciaService(IInteligenciaRepository repo)
        {
            _repo = repo;
        }

        public Task<IReadOnlyList<TraidorDto>> GetTraidoresAsync(string faccion)
        {
            if (string.IsNullOrWhiteSpace(faccion))
                throw new ArgumentException("Faccion es obligatoria");

            return _repo.GetTraidoresAsync(faccion);
        }
    }
}