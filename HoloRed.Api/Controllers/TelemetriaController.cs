using HoloRed.Application.Services;
using HoloRed.Domain.Exceptions;
using HoloRed.Domain.Telemetria;
using Microsoft.AspNetCore.Mvc;

namespace HoloRed.Api.Controllers
{
    [ApiController]
    [Route("telemetria")]
    public class TelemetriaController : ControllerBase
    {
        private readonly TelemetriaService _service;

        public TelemetriaController(TelemetriaService service)
        {
            _service = service;
        }

        [HttpPost("impacto")]
        public async Task<IActionResult> PostImpacto([FromBody] Impacto impacto)
        {
            try
            {
                await _service.InsertImpactoAsync(impacto);
                return Ok(new { message = "Impacto registrado" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "BAD_REQUEST", message = ex.Message });
            }
            catch (ExternalServiceUnavailableException ex)
            {
                return StatusCode(503, new { error = ex.ErrorCode, message = ex.Message, service = ex.ServiceName });
            }
        }

        [HttpGet("historial/{sectorId}")]
        public async Task<IActionResult> GetHistorial(string sectorId, [FromQuery] string fecha)
        {
            try
            {
                if (!DateOnly.TryParse(fecha, out var date))
                    return BadRequest(new { error = "BAD_REQUEST", message = "fecha debe ser YYYY-MM-DD" });

                var res = await _service.GetHistorialAsync(sectorId, date);
                return Ok(res);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "BAD_REQUEST", message = ex.Message });
            }
            catch (ExternalServiceUnavailableException ex)
            {
                return StatusCode(503, new { error = ex.ErrorCode, message = ex.Message, service = ex.ServiceName });
            }
        }
    }
}