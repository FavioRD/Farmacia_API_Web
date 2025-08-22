using FarmaciaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace FarmaciaWeb.Controllers
{
    public class ProductosController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductosController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FarmaciaApi");
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            try
            {
                var productos = await _httpClient.GetFromJsonAsync<List<Producto>>("api/Productos");
                return View(productos ?? new List<Producto>());
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", $"Error de conexión con la API: {ex.Message}");
                return View(new List<Producto>());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error inesperado: {ex.Message}");
                return View(new List<Producto>());
            }
        }

        // GET: Productos/Create
        public IActionResult Create() => View();

        // POST: Productos/Create
        [HttpPost]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (!ModelState.IsValid)
                return View(producto);

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Productos", producto);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Error al registrar producto en la API");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error inesperado: {ex.Message}");
            }

            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var producto = await _httpClient.GetFromJsonAsync<Producto>($"api/Productos/{id}");
                if (producto == null)
                    return NotFound();

                return View(producto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al obtener producto: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Productos/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Producto producto)
        {
            if (!ModelState.IsValid)
                return View(producto);

            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Productos/{producto.Id}", producto);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Error al actualizar producto en la API");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error inesperado: {ex.Message}");
            }

            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var producto = await _httpClient.GetFromJsonAsync<Producto>($"api/Productos/{id}");
                if (producto == null)
                    return NotFound();

                return View(producto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al obtener producto: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Productos/Delete
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Productos/{id}");
                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Error al eliminar producto en la API");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error inesperado: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
