using Library_Web_Api.Models;
using Library_Web_Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library_Web_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet("GetAllAuthors")]
        public async Task<IActionResult> GetAllAuthors()
        {
            try
            {
                var authors = await _authorService.GetAllAuthors();
                return Ok(authors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("{authorId}/GetAuthorById")]
        public async Task<IActionResult> GetAuthorById(int authorId)
        {
            try
            {
                var author = await _authorService.GetAuthorById(authorId);
                if (author == null)
                {
                    return NotFound(new { Message = $"Author with ID {authorId} not found." });
                }
                return Ok(author);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("GetAuthorByName/{authorName}")]
        public async Task<IActionResult> GetAuthorByName(string authorName)
        {
            try
            {
                var author = await _authorService.GetAuthorByName(authorName);
                if (author == null)
                {
                    return NotFound(new { Message = $"Author with name {authorName} not found." });
                }
                return Ok(author);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
