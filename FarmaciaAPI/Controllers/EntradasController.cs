using FarmaciaAPI.IServices;
using FarmaciaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FarmaciaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntradasController : ControllerBase
    {
        private readonly IEntradasService _service;

        public EntradasController(IEntradasService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingreso>>> GetAll()
        {
            var ingresos = await _service.ListarIngresosAsync();
            return Ok(ingresos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ingreso>> GetById(int id)
        {
            var ingreso = await _service.ObtenerIngresoPorIdAsync(id);
            if (ingreso == null)
                return NotFound();

            return Ok(ingreso);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ingreso ingreso)
        {
            await _service.RegistrarIngresoAsync(ingreso);
            return Ok(new { message = "Ingreso registrado correctamente" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Ingreso ingreso)
        {
            if (id != ingreso.Id)
                return BadRequest();

            await _service.ActualizarIngresoAsync(ingreso);
            return Ok(new { message = "Ingreso actualizado correctamente" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.EliminarIngresoAsync(id);
            return Ok(new { message = "Ingreso eliminado correctamente" });
        }
    }
}

