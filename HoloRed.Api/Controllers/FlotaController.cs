using HoloRed.Api.Models;
using HoloRed.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace HoloRed.Api.Controllers
{
    [ApiController]
    [Route("flota")]
    public class FlotaController : ControllerBase
    {
        private readonly DockingService _docking;

        public FlotaController(DockingService docking)
        {
            _docking = docking;
        }

        [HttpPost("desatraque")]
        public async Task<IActionResult> Desatraque([FromBody] UndockRequest request)
        {
            var codigo = (request.CodigoNave ?? "").Trim();
            if (string.IsNullOrWhiteSpace(codigo))
                return BadRequest(new { error = "CODIGO_NAVE_REQUERIDO" });

            var result = await _docking.LiberarAtraqueAsync(codigo);

            if (!result.ok && result.error == "NO_ATRACADA")
                return NotFound(new { error = "NO_ATRACADA", message = "La nave no estaba atracada" });

            return Ok(new { codigoNave = codigo, bahiaLiberada = result.bahia });
        }

        [HttpPost("atraque")]
        public async Task<IActionResult> Atraque([FromBody] DockRequest request)
        {
            var codigo = (request.CodigoNave ?? "").Trim();
            if (string.IsNullOrWhiteSpace(codigo))
                return BadRequest(new { error = "CODIGO_NAVE_REQUERIDO" });

            var result = await _docking.SolicitarAtraqueAsync(codigo);

            // Ajusta estos if según tu DockingService (te lo explico abajo)
            if (!result.ok && result.error == "NO_HAY_BAHIAS")
                return Conflict(new { error = "NO_HAY_BAHIAS", message = "No hay bahías disponibles" });

            return Ok(new { codigoNave = codigo, bahiaAsignada = result.bahia });
        }
    }
}