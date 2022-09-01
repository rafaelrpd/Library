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

        // GET: api/Searches/books?categoryName={string}&authorName={string}&bookTitle={string}
        /// <summary>
        /// Get books by category name(string), author name(string) or book title(string), or by a combination of all of those.
        /// </summary>
        /// <returns>Get books by category name(string), author name(string) or book title(string), or by a combination of all of those.</returns>
        /// <remarks>
        /// Instructions: Just send a GET request to URI /api/searches/books?{params}, where "params" could be : 
        ///     categoryName={string}
        ///     authorName={string}
        ///     bookTitle={string}
        ///     or using "&amp;" to concatenate:
        ///     
        ///     categoryName={string}&amp;authorName={string}&amp;bookTitle={string}
        /// </remarks>
        /// <param name="categoryName" example="Tech"></param>
        /// <param name="authorName" example="Maycon"></param>
        /// <param name="bookTitle" example="Borland"></param>
        /// <response code="200">Book list returned successfully</response>
        /// <response code="404">Book list not found.</response>
        [HttpGet("books")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooksByCategory(string? categoryName, string? authorName, string? bookTitle)
        {
            if (_context.Categories == null || _context.Authors == null || _context.Books == null)
            {
                return NotFound();
            }

            var query = _context.Books.AsQueryable();

            if (categoryName != null)
            {
                query = query.Where(cn => cn.Category.Name.Contains(categoryName));
            }

            if (authorName != null)
            {
                query = query.Where(cn => cn.Author.Name.Contains(authorName));
            }

            if (bookTitle != null)
            {
                query = query.Where(cn => cn.Title.Contains(bookTitle));
            }


            List<BookDTO> result = await query
                .Select(b => BookToDTO(b))
                .ToListAsync();

            if (result == null)
            {
                return NotFound();
            }

            return result;
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
