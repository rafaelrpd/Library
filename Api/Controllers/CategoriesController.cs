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
    public class CategoriesController : ControllerBase
    {
        private readonly LIBRARYContext _context;

        public CategoriesController(LIBRARYContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }

            return await _context.Categories
                .Select(c => CategoryToDTO(c))
                .ToListAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Where(i => i.CategoryId == id)
                .Include(b => b.Books)
                .Select(c => CategoryDetailsToDTO(c))
                .ToListAsync();

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, CategoryDTO categoryDTO)
        {
            if (id != categoryDTO.CategoryId)
            {
                return BadRequest();
            }

            ICollection<Book> books = _context.Books.Where(ci => ci.CategoryId == categoryDTO.CategoryId).ToList();
            Category category = new Category()
            {
                CategoryId = categoryDTO.CategoryId,
                Name = categoryDTO.Name,
                Books = books
            };
            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(CategoryDTO categoryDTO)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'LIBRARYContext.Categories'  is null.");
            }

            Category category = new Category()
            {
                Name = categoryDTO.Name,
                Books = null!
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            categoryDTO.CategoryId = category.CategoryId;
            return CreatedAtAction(nameof(GetCategory), new { id = categoryDTO.CategoryId }, categoryDTO);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }

        static CategoryDTO CategoryToDTO(Category category) =>
            new CategoryDTO
            {
                CategoryId = category.CategoryId,
                Name = category.Name
            };
        static CategoryDetailsDTO CategoryDetailsToDTO(Category category) =>
            new CategoryDetailsDTO
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Books = category.Books.Select(b => BookToDTO(b)).ToList()
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
