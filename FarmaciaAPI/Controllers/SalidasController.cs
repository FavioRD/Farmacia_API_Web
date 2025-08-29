using FarmaciaAPI.IServices;
using FarmaciaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FarmaciaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalidasController : ControllerBase
    {
        private readonly ISalidaService _service;

        public SalidasController(ISalidaService service)
        {
            _service = service;
        }

        // GET: api/Salidas
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var salidas = await _service.Listar();
                return Ok(salidas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
            }
        }

        // GET: api/Salidas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var salida = await _service.ObtenerPorId(id);
            if (salida == null) return NotFound();
            return Ok(salida);
        }

        // POST: api/Salidas
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Salida salida)
        {
            try
            {
                if (salida.Cantidad <= 0)
                    return BadRequest("La cantidad debe ser mayor a 0");

                await _service.Registrar(salida.ProductoId, salida.Cantidad);
                return Ok(new { mensaje = "Salida registrada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT: api/Salidas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Salida salida)
        {
            var actualizado = await _service.Actualizar(id, salida);
            if (!actualizado) return NotFound();
            return Ok(new { mensaje = "Salida actualizada correctamente" });
        }

        // DELETE: api/Salidas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _service.Eliminar(id);
            if (!eliminado) return NotFound();
            return Ok(new { mensaje = "Salida eliminada correctamente" });
        }
    }
}
