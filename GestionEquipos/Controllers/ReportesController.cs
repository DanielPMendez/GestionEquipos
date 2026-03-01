using GestionEquipos.ServiceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionEquipos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportesController : ControllerBase
    {
        private readonly IReportesService _service;

        public ReportesController(IReportesService service) => _service = service;

        [HttpGet("EquiposPorCategoria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEquiposPorCategoria(
            int page = 1,
            int pageSize = 10,
            string? nombre = null,
            string? marca = null,
            int? idProveedor = null,
            string sortBy = "Nombre",
            string sortDir = "ASC")
        {
            try
            {
                var result = await _service.ObtenerEquiposPorCategoriaAsync(page, pageSize, nombre, marca, idProveedor, sortBy, sortDir);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("EquiposTopRentabilidad")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEquiposTopRentabilidad(
            int page = 1,
            int pageSize = 10,
            decimal? minPrecio = null,
            decimal? maxPrecio = null,
            string sortBy = "Precio",
            string sortDir = "DESC")
        {
            try
            {
                var result = await _service.ObtenerEquiposTopRentabilidadAsync(page, pageSize, minPrecio, maxPrecio, sortBy, sortDir);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
