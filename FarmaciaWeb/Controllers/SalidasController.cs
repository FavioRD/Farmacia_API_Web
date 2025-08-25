using FarmaciaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;

namespace FarmaciaWeb.Controllers
{
    public class SalidasController : Controller
    {
        private readonly HttpClient _httpClient;

        public SalidasController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FarmaciaApi");
        }

        // GET: Salidas
        public async Task<IActionResult> Index()
        {
            var salidas = await _httpClient.GetFromJsonAsync<List<Salida>>("api/Salidas");
            var productos = await _httpClient.GetFromJsonAsync<List<Producto>>("api/Productos");

            // Vincular Producto con cada salida
            foreach (var salida in salidas)
            {
                salida.Producto = productos.FirstOrDefault(p => p.Id == salida.ProductoId);
            }

            return View(salidas);
        }


        // GET: Salidas/Create
        public async Task<IActionResult> Create()
        {
            var productos = await _httpClient.GetFromJsonAsync<List<Producto>>("api/Productos");

            ViewBag.Productos = new SelectList(productos, "Id", "Nombre");

            return View();
        }

        // POST: Salidas/Create
        [HttpPost]
        public async Task<IActionResult> Create(Salida salida)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Salidas", salida);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Error al registrar salida");
            return View(salida);
        }

        // GET: Salidas/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var salida = await _httpClient.GetFromJsonAsync<Salida>($"api/Salidas/{id}");
            if (salida == null)
                return NotFound();

            return View(salida);
        }

        // POST: Salidas/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Salida salida)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Salidas/{salida.Id}", salida);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Error al actualizar salida");
            return View(salida);
        }

        // GET: Salidas/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var salida = await _httpClient.GetFromJsonAsync<Salida>($"api/Salidas/{id}");
            if (salida == null)
                return NotFound();

            return View(salida);
        }

        // POST: Salidas/Delete
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Salidas/{id}");
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Error al eliminar salida");
            return RedirectToAction(nameof(Index));
        }


    }
}
