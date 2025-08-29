using FarmaciaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace FarmaciaWeb.Controllers
{
    public class EntradasController : Controller
    {
        private readonly HttpClient _httpClient;

        public EntradasController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FarmaciaApi");
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _httpClient.GetFromJsonAsync<List<Entrada>>("api/Entradas");
            return View(productos);
        }

        // GET: Entradas/Create
        public async Task<IActionResult> Create()
        {
            var productos = await _httpClient.GetFromJsonAsync<List<Producto>>("api/Productos");
            ViewBag.Productos = new SelectList(productos, "Id", "Nombre");

            return View();
        }

        // POST: Entradas/Create
        [HttpPost]
        public async Task<IActionResult> Create(Entrada entrada)
        {
            // Obtener lista de productos para buscar el nombre
            var productos = await _httpClient.GetFromJsonAsync<List<Producto>>("api/Productos");
            var productoSeleccionado = productos.FirstOrDefault(p => p.Id == entrada.ProductoId);
            if (productoSeleccionado != null)
            {
                entrada.Producto = productoSeleccionado.Nombre; // asigno el nombre
            }

            var response = await _httpClient.PostAsJsonAsync("api/Entradas", entrada);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Error al registrar entrada");
            return View(entrada);
        }

        // GET: Entradas/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var entrada = await _httpClient.GetFromJsonAsync<Entrada>($"api/Entradas/{id}");
            if (entrada == null)
                return NotFound();

            return View(entrada);
        }

        // POST: Entradas/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Entrada entrada)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Entradas/{entrada.Id}", entrada);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Error al actualizar entrada");
            return View(entrada);
        }

        // GET: Entradas/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var entrada = await _httpClient.GetFromJsonAsync<Entrada>($"api/Entradas/{id}");
            if (entrada == null)
                return NotFound();

            return View(entrada);
        }

        // POST: Entradas/Delete
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Entradas/{id}");
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Error al eliminar entrada");
            return RedirectToAction(nameof(Index));
        }
    }
}
