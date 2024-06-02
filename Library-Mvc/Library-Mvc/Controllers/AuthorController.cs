using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Library_Mvc.Models;

namespace Library_Mvc.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public AuthorController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var client = _clientFactory.CreateClient("Library-Web-Api");
                var response = await client.GetAsync("Author/GetAllAuthors");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var authors = JsonConvert.DeserializeObject<List<Author>>(json);
                    return View(authors);
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
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

            try
            {
                var client = _clientFactory.CreateClient("Library-Web-Api");
                var response = await client.GetAsync($"Author/GetAuthorByName/{searchQuery}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var author = JsonConvert.DeserializeObject<Author>(json);
                    if (author != null)
                    {
                        return View("Index", new List<Author> { author });
                    }
                    else
                    {
                        ViewBag.Message = $"Author with name '{searchQuery}' not found.";
                        return View("Index", new List<Author>());
                    }
                }
                else
                {
                    ViewBag.Message = $"Author with name '{searchQuery}' not found.";
                    return View("Index", new List<Author>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Internal Server Error: {ex.Message}";
                return View("Index", new List<Author>());
            }
        }
    }
}