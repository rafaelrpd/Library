using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchesController : ControllerBase
    {
        private readonly LIBRARYContext _context;

        public SearchesController(LIBRARYContext context)
        {
            _context = context;
        }

        // GET api/Searches/5
        [HttpGet("books/by/category/{categoryName}")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooksByCategory(string categoryName)
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }

            return await _context.Books
                .Where(cn => cn.Category.Name == categoryName)
                .Select(b => BookToDTO(b))
                .ToListAsync();
        }

        //// POST api/Searches
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/Searches/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/Searches/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        static BookDTO BookToDTO(Book book) =>
            new BookDTO
            {
                Isbn = book.Isbn,
                AuthorId = book.AuthorId,
                CategoryId = book.CategoryId,
                Title = book.Title,
                Quantity = book.Quantity
            };
    }
}
