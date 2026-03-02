using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HoloRed.Application.Services
{
    public class DockingService
    {
        private readonly SemaphoreSlim _mutex = new(1, 1);
        private readonly Queue<int> _bahiasLibres;
        private readonly Dictionary<string, int> _asignadas;

        public DockingService(int numeroBahias = 5)
        {
            _bahiasLibres = new Queue<int>();
            for (int i = 1; i <= numeroBahias; i++)
                _bahiasLibres.Enqueue(i);

            _asignadas = new Dictionary<string, int>();
        }

        public async Task<(bool ok, int? bahia, string? error)> SolicitarAtraqueAsync(string codigoNave)
        {
            await _mutex.WaitAsync();
            try
            {
                if (_asignadas.ContainsKey(codigoNave))
                    return (false, null, "YA_ATRACADA");

                if (_bahiasLibres.Count == 0)
                    return (false, null, "SIN_BAHIAS");

                var bahia = _bahiasLibres.Dequeue();
                _asignadas[codigoNave] = bahia;

                return (true, bahia, null);
            }
            finally
            {
                _mutex.Release();
            }
        }

        public async Task<(bool ok, int? bahia, string? error)> LiberarAtraqueAsync(string codigoNave)
        {
            await _mutex.WaitAsync();
            try
            {
                if (!_asignadas.TryGetValue(codigoNave, out var bahia))
                    return (false, null, "NO_ATRACADA");

                _asignadas.Remove(codigoNave);
                _bahiasLibres.Enqueue(bahia);

                return (true, bahia, null);
            }
            finally
            {
                _mutex.Release();
            }
        }
    }
}