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
    public class AuthorsController : ControllerBase
    {
        private readonly LIBRARYContext _context;

        public AuthorsController(LIBRARYContext context)
        {
            _context = context;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAuthors()
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }
            return await _context.Authors
                .Select(a => AuthorToDTO(a))
                .ToListAsync();
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }
            var author = await _context.Authors
                .Where(i => i.AuthorId == id)
                .Include(b => b.Books)
                .Select(a => AuthorDetailsToDTO(a))
                .ToListAsync();

            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorDTO authorDTO)
        {
            if (id != authorDTO.AuthorId)
            {
                return BadRequest();
            }

            ICollection<Book> books = _context.Books.Where(ai => ai.AuthorId == authorDTO.AuthorId).ToList();
            Author author = new Author()
            {
                AuthorId = authorDTO.AuthorId,
                Name = authorDTO.Name,
                RegistrationDate = authorDTO.RegistrationDate,
                Books = books
            };
            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(AuthorDTO authorDTO)
        {
            if (_context.Authors == null)
            {
                return Problem("Entity set 'LIBRARYContext.Authors'  is null.");
            }

            Author author = new Author()
            {
                Name = authorDTO.Name,
                RegistrationDate = authorDTO.RegistrationDate,
                Books = null!
            };
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            authorDTO.AuthorId = author.AuthorId;
            return CreatedAtAction(nameof(GetAuthor), new { id = authorDTO.AuthorId }, authorDTO);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return (_context.Authors?.Any(e => e.AuthorId == id)).GetValueOrDefault();
        }

        static AuthorDTO AuthorToDTO(Author author) =>
            new AuthorDTO
            {
                AuthorId = author.AuthorId,
                Name = author.Name,
                RegistrationDate = author.RegistrationDate
            };

        static AuthorDetailsDTO AuthorDetailsToDTO(Author author) =>
            new AuthorDetailsDTO
            {
                AuthorId = author.AuthorId,
                Name = author.Name,
                RegistrationDate = author.RegistrationDate,
                Books = author.Books.Select(b => BookToDTO(b)).ToList()
            };

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
