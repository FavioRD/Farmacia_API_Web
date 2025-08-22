using FarmaciaApi.Services;
using FarmaciaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FarmaciaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalidasController : ControllerBase
    {
        private readonly SalidaService _service;

        public SalidasController(SalidaService service)
        {
            _service = service;
        }

        // GET: api/Salidas
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.Listar());

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
            await _service.Registrar(salida.ProductoId, salida.Cantidad);
            return Ok(new { mensaje = "Salida registrada correctamente" });
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
