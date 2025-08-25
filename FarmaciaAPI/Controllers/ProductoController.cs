using FarmaciaAPI.Services;
using FarmaciaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FarmaciaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ProductoService _service;

        public ProductosController(ProductoService service)
        {
            _service = service;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.Listar());

        // GET: api/Productos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var producto = await _service.ObtenerPorId(id);
            if (producto == null) return NotFound();
            return Ok(producto);
        }

        // POST: api/Productos
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Producto p)
        {
            await _service.Insertar(p);
            return Ok(new { mensaje = "Producto insertado correctamente" });
        }

        // PUT: api/Productos/5 
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Producto p)
        {
            var actualizado = await _service.Actualizar(id, p);
            if (!actualizado) return NotFound();
            return Ok(new { mensaje = "Producto actualizado correctamente" });
        }

        // DELETE: api/Productos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _service.Eliminar(id);
            if (!eliminado) return NotFound();
            return Ok(new { mensaje = "Producto eliminado correctamente" });
        }
    }
}
