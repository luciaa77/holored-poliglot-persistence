using HoloRed.Api.Models;
using HoloRed.Infrastructure.Redis;
using Microsoft.AspNetCore.Mvc;

namespace HoloRed.Api.Controllers
{
    [ApiController]
    [Route("radar")]
    public class RadarController : ControllerBase
    {
        private readonly RedisConnection _redis;

        public RadarController(RedisConnection redis)
        {
            _redis = redis;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new { ok = true, module = "radar" });
        }

        [HttpPost("baliza/{codigo_nave}")]
        public async Task<IActionResult> Baliza(string codigo_nave, [FromBody] BeaconRequest request)
        {
            // Validación simple
            var estado = (request.Estado ?? "").Trim().ToLowerInvariant();
            if (estado != "patrulla" && estado != "hiperespacio" && estado != "combate")
                return BadRequest(new { error = "ESTADO_INVALIDO", allowed = new[] { "patrulla", "hiperespacio", "combate" } });

            var key = $"nave:{codigo_nave}:estado";

            // TTL 10 minutos
            var ttl = TimeSpan.FromMinutes(10);

            await _redis.Database.StringSetAsync(key, estado, ttl);

            return Ok(new { codigo = codigo_nave, estado, ttlSeconds = (int)ttl.TotalSeconds });
        }
    }
}