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
        /// <summary>
        /// List all categories with categoryId and name.
        /// </summary>
        /// <returns>List all categories with categoryId and name.</returns>
        /// <remarks>
        /// Instructions: Just send a GET request to URI /api/categories.
        /// </remarks>
        /// <response code="200">Returns all categories with simple information.</response>
        /// <response code="404">Category list not found.</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <summary>
        /// Return a specific category with full information.
        /// </summary>
        /// <returns>Return a specific category with full information.</returns>
        /// <remarks>
        /// Instructions: Just send a GET request to URI /api/categories/{id}, where ID is an INT.
        ///     Sample request:
        ///     
        ///         GET /api/categories/1
        /// </remarks>
        /// <param name="id" example="1"></param>
        /// <response code="200">Return a specific category with full information.</response>
        /// <response code="404">Category not found.</response>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDetailsDTO>> GetCategory(int id)
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }

            CategoryDetailsDTO category = await _context.Categories
                .Where(i => i.CategoryId == id)
                .Include(b => b.Books)
                .Select(c => CategoryDetailsToDTO(c))
                .FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Change all info of a specific category.
        /// </summary>
        /// <param name="id" example="1"></param>
        /// <param name="categoryDTO"></param>
        /// <returns>Change all information from a specific category.</returns>
        /// <remarks>
        /// Instructions: Send a PUT request to URI /api/categories/{id}, where ID is an INT with the following body as JSON.
        ///     Sample request:
        ///     
        ///         PUT /api/categories/1
        ///         {
        ///             "categoryId": 1,
        ///             "name": "New category name"
        ///         }
        /// </remarks>
        /// <response code="204">Changes done correctly.</response>
        /// <response code="400">URI ID not equal as informed in JSON body.</response>
        /// <response code="404">Category not found.</response>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutCategory(int id, CategoryDTO categoryDTO)
        {
            if (id != categoryDTO.CategoryId)
            {
                return BadRequest();
            }

            Category categoryContext = await _context.Categories.FirstOrDefaultAsync(ci => ci.CategoryId == id);
            if (categoryContext == null)
            {
                return NotFound();
            }
            categoryContext.CategoryId = categoryDTO.CategoryId;
            categoryContext.Name = categoryDTO.Name;

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
        /// <summary>
        /// Create a new category.
        /// </summary>
        /// <param name="categoryPostDTO"></param>
        /// <returns>Create a new category.</returns>
        /// <remarks>
        /// Instructions: Send a POST request to URI /api/categories with the following body as JSON.
        ///     Sample request:
        ///     
        ///     POST /api/categories
        ///         {
        ///             "name": "New category name"
        ///         }
        /// </remarks>
        /// <response code="204">New category created successfully</response>
        /// <response code="404">Entity set 'LIBRARYContext.Categories' is null.</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PostCategory(CategoryPostDTO categoryPostDTO)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'LIBRARYContext.Categories'  is null.");
            }

            Category category = new Category()
            {
                Name = categoryPostDTO.Name
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Categories/5
        /// <summary>
        /// Delete a specific category.
        /// </summary>
        /// <param name="id" example="1007"></param>
        /// <returns>Delete a specific category by ID.</returns>
        /// <remarks>
        /// Instructions: Just send a DELETE request to URI /api/categories/{id}, where ID is an INT.
        ///     Sample request:
        ///     
        ///         DELETE /api/category/1007
        /// </remarks>
        /// <response code="204">Delete done successfully.</response>
        /// <response code="404">Author not found.</response>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }
            Category category = await _context.Categories.FindAsync(id);
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
