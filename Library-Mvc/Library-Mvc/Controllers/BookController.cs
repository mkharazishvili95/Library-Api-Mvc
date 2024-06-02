using Library_Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Library_Mvc.Controllers
{
    public class BooksController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public BooksController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("https://localhost:7208/api/Book/GetAllBooks-Including-Authors");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var books = JsonConvert.DeserializeObject<List<Book>>(json);
                return View(books);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                return RedirectToAction("Index");
            }
            using var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7208/api/Book/By-Name/{searchQuery}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var books = new List<Book> { JsonConvert.DeserializeObject<Book>(json) };
                return View("Index", books);
            }
            else
            {
                ViewBag.Message = $"Book with title '{searchQuery}' not found.";
                return View("Index",new List<Book>());
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(int bookId)
        {
            using var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7208/api/Book/{bookId}/GetBookById-including-authors");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var book = JsonConvert.DeserializeObject<Book>(json);
                return View(book);
            }
            else
            {
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(int bookId)
        {
            using var client = _clientFactory.CreateClient();
            var response = await client.PutAsync($"https://localhost:7208/api/Book/{bookId}/TakingOutTheBook", null);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { bookId });
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Return(int bookId)
        {
            using var client = _clientFactory.CreateClient();
            var response = await client.PutAsync($"https://localhost:7208/api/Book/{bookId}/ReturnTheBook", null);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { bookId });
            }
            else
            {
                return View("Error");
            }
        }
        public IActionResult AddNewBook()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.DeleteAsync($"https://localhost:7208/api/Book/{id}/DeleteBook");
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "The book was successfully deleted.";
                    return RedirectToAction("Index", "Books", new { area = "" });
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = errorMessage;
                    return RedirectToAction("Index", "Books", new { area = "" });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Internal Server Error: {ex.Message}";
                return RedirectToAction("Index", "Books", new { area = "" });
            }
        }

        

        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] Book newBook)
        {
            try
            {
                var json = JsonConvert.SerializeObject(newBook);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                using var client = _clientFactory.CreateClient();
                var response = await client.PostAsync("https://localhost:7208/api/Book/AddNewBook", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Book has successfully added.";
                    return RedirectToAction("Index", "Books");
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    var errors = JsonConvert.DeserializeObject<string[]>(errorResponse);
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Internal Server Error: {ex.Message}");
                return View(); 
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateBook(int bookId)
        {
            using var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7208/api/Book/{bookId}/GetBookById-including-authors");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var book = JsonConvert.DeserializeObject<Book>(json);
                return View(book);
            }
            else
            {
                ViewBag.ErrorMessage = "Error fetching book details.";
                return View("Error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBook(int bookId, [FromBody] Book updatedBook)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                using var client = _clientFactory.CreateClient();
                var json = JsonConvert.SerializeObject(updatedBook);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"https://localhost:7208/api/Book/{bookId}/UpdateBook", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Book has been updated successfully.";
                    return Ok(new { success = true, message = "Book has been updated successfully." });
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    return BadRequest(new { success = false, errors = errorResponse });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Internal Server Error: {ex.Message}" });
            }
        }
    }
}

