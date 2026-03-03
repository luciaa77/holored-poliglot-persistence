using HoloRed.Application.Services;
using HoloRed.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace HoloRed.Api.Controllers
{
    [ApiController]
    [Route("inteligencia")]
    public class InteligenciaController : ControllerBase
    {
        private readonly InteligenciaService _service;

        public InteligenciaController(InteligenciaService service)
        {
            _service = service;
        }

        [HttpGet("{faccion}/traidores")]
        public async Task<IActionResult> GetTraidores(string faccion)
        {
            try
            {
                var res = await _service.GetTraidoresAsync(faccion);
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