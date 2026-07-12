using candidato.Controllers.Fachadas;
using candidatos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace candidato.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IFachadaAuth _authFachada;

        public AdminController(IFachadaAuth authFachada)
        {
            _authFachada = authFachada;
        }

        [HttpPost("recrutadores")]
        public async Task<IActionResult> CriarRecrutador([FromBody] RegisterDto dto)
        {
            try
            {
                var result = await _authFachada.RegistrarRecrutador(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}
