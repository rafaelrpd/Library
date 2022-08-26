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
        /// <summary>
        /// List all authors with id, name and registration date.
        /// </summary>
        /// <returns>List of all authors with id, name and registration date.</returns>
        /// <remarks>
        /// Instructions: Just send a GET request to URI /api/authors.
        /// </remarks>
        /// <response code="200">Returns all authors with simple information.</response>
        /// <response code="404">Authors list not found.</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <summary>
        /// Return a specific author with full information.
        /// </summary>
        /// <returns>Return a specific author with full information.</returns>
        /// <remarks>
        /// Instructions: Just send a GET request to URI /api/authors/{id}, where ID is an INT.
        ///     Sample request:
        ///     
        ///         GET /api/authors/5
        /// </remarks>
        /// <param name="id" example="1"></param>
        /// <response code="200">Return a specific author with full information.</response>
        /// <response code="404">Author not found.</response>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthorDetailsDTO>> GetAuthor(int id)
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }

            AuthorDetailsDTO author = await _context.Authors
                .Where(i => i.AuthorId == id)
                .Include(b => b.Books)
                .Select(a => AuthorDetailsToDTO(a))
                .FirstOrDefaultAsync();

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Change all info of a specific author.
        /// </summary>
        /// <param name="id" example="5"></param>
        /// <param name="authorDTO"></param>
        /// <returns>Change all information from a specific author.</returns>
        /// <remarks>
        /// Instructions: Send a PUT request to URI /api/authors/{id}, where ID is an INT with the following body as JSON.
        ///     Sample request:
        ///     
        ///         PUT /api/authors/5
        ///         {
        ///             "authorId": 5,
        ///             "name": "Darth Vader"
        ///         }
        /// </remarks>
        /// <response code="204">Changes done correctly.</response>
        /// <response code="400">URI ID not equal as informed in JSON body.</response>
        /// <response code="404">Author not found.</response>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAuthor(int id, AuthorDTO authorDTO)
        {
            if (id != authorDTO.AuthorId)
            {
                return BadRequest();
            }

            Author authorContext = await _context.Authors.FirstOrDefaultAsync(ai => ai.AuthorId == id);
            if (authorContext == null)
            {
                return NotFound();
            }
            authorContext.Name = authorDTO.Name;

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
        /// <summary>
        /// Create a new author.
        /// </summary>
        /// <param name="authorPostDTO"></param>
        /// <returns>Create a new author.</returns>
        /// <remarks>
        /// Instructions: Send a POST request to URI /api/authors with the following body as JSON.
        ///     Sample request:
        ///     
        ///     POST /api/authors
        ///     {
        ///         "name": "Darth Maul"
        ///     }
        /// </remarks>
        /// <response code="204">New author created successfully</response>
        /// <response code="404">Entity set 'LIBRARYContext.Authors' is null.</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PostAuthor(AuthorPostDTO authorPostDTO)
        {
            if (_context.Authors == null)
            {
                return Problem("Entity set 'LIBRARYContext.Authors'  is null.");
            }

            Author author = new Author()
            {
                Name = authorPostDTO.Name
            };
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Authors/5
        /// <summary>
        /// Delete a specific author.
        /// </summary>
        /// <param name="id" example="1022"></param>
        /// <returns>Delete a specific author by ID.</returns>
        /// <remarks>
        /// Instructions: Just send a DELETE request to URI /api/authors/{id}, where ID is an INT.
        ///     Sample request:
        ///     
        ///         DELETE /api/authors/1022
        /// </remarks>
        /// <response code="204">Delete done successfully.</response>
        /// <response code="404">Author not found.</response>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }
            Author author = await _context.Authors.FindAsync(id);
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
                Name = author.Name
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
