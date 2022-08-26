using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LIBRARYContext _context;

        public BooksController(LIBRARYContext context)
        {
            _context = context;
        }

        // GET: api/Books
        /// <summary>
        /// List all books with ISBN, authorId, categoryId, title and quantity.
        /// </summary>
        /// <returns>List all books with ISBN, authorId, categoryId, title and quantity.</returns>
        /// <remarks>
        /// Instructions: Just send a GET request to URI /api/books.
        /// </remarks>
        /// <response code="200">Returns all books with simple information.</response>
        /// <response code="404">Book list not found.</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            return await _context.Books
                .Select(b => BookToDTO(b))
                .ToListAsync();
        }

        // GET: api/Books/8886713611511
        /// <summary>
        /// Return a specific book with full information.
        /// </summary>
        /// <returns>Return a specific book with full information.</returns>
        /// <remarks>
        /// Instructions: Just send a GET request to URI /api/books/{id}, where ID is an ISBN as INT(13).
        ///     Sample request:
        ///     
        ///         GET /api/books/8886713611511
        /// </remarks>
        /// <param name="id" example="8886713611511"></param>
        /// <response code="200">Return a specific book with full information.</response>
        /// <response code="404">Book not found.</response>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDetailsDTO>> GetBook(string id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }

            BookDetailsDTO book = await _context.Books
                .Where(i => i.Isbn == id)
                .Include(a => a.Author)
                .Include(c => c.Category)
                .Select(b => BookDetailsToDTO(b))
                .FirstOrDefaultAsync();

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/8886713611511
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Change all info of a specific book.
        /// </summary>
        /// <param name="id" example="8886713611511"></param>
        /// <param name="bookDTO"></param>
        /// <returns>Change all information from a specific book.</returns>
        /// <remarks>
        /// Instructions: Send a PUT request to URI /api/books/{id}, where ID is an ISBN as INT(13) with the following body as JSON.
        ///     Sample request:
        ///     
        ///         PUT /api/books/8886713611511
        ///         {
        ///             "isbn": 8886713611511,
        ///             "authorId": "1",
        ///             "categoryId": "1",
        ///             "title": "New title",
        ///             "quantity": 10
        ///         }
        /// </remarks>
        /// <response code="204">Changes done correctly.</response>
        /// <response code="400">URI ID not equal as informed in JSON body.</response>
        /// <response code="404">Book not found.</response>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutBook(string id, BookDTO bookDTO)
        {
            if (id != bookDTO.Isbn)
            {
                return BadRequest();
            }

            Book bookContext = await _context.Books.FirstOrDefaultAsync(bi => bi.Isbn == id);
            if (bookContext == null)
            {
                return NotFound();
            }
            bookContext.AuthorId = bookDTO.AuthorId;
            bookContext.CategoryId = bookDTO.CategoryId;
            bookContext.Title = bookDTO.Title;
            bookContext.Quantity = bookDTO.Quantity;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a new book.
        /// </summary>
        /// <param name="bookDTO"></param>
        /// <returns>Create a new book.</returns>
        /// <remarks>
        /// Instructions: Send a POST request to URI /api/books with the following body as JSON.
        ///     Sample request:
        ///     
        ///     POST /api/books
        ///         {
        ///             "isbn": 8886713611511,
        ///             "authorId": "3",
        ///             "categoryId": "3",
        ///             "title": "New post book",
        ///             "quantity": 30
        ///         }
        /// </remarks>
        /// <response code="204">New author created successfully</response>
        /// <response code="404">Entity set 'LIBRARYContext.Books' is null.</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PostBook(BookDTO bookDTO)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'LIBRARYContext.Books'  is null.");
            }

            Author author = _context.Authors.Where(ai => ai.AuthorId == bookDTO.AuthorId).First();
            Category category = _context.Categories.Where(ai => ai.CategoryId == bookDTO.CategoryId).First();
            Book book = new Book()
            {
                Isbn = bookDTO.Isbn,
                AuthorId = bookDTO.AuthorId,
                CategoryId = bookDTO.CategoryId,
                Title = bookDTO.Title,
                Quantity = bookDTO.Quantity,
                Author = author,
                Category = category,
                BorrowedBooks = null!
            };
            _context.Books.Add(book);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BookExists(book.Isbn))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Books/8886713611511
        /// <summary>
        /// Delete a specific book.
        /// </summary>
        /// <param name="id" example="8886713611511"></param>
        /// <returns>Delete a specific book by ISBN.</returns>
        /// <remarks>
        /// Instructions: Just send a DELETE request to URI /api/books/{id}, where ID is an ISBN as INT(13).
        ///     Sample request:
        ///     
        ///         DELETE /api/books/8886713611511
        /// </remarks>
        /// <response code="204">Delete done successfully.</response>
        /// <response code="404">Book not found.</response>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBook(string id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(string id)
        {
            return (_context.Books?.Any(e => e.Isbn == id)).GetValueOrDefault();
        }

        static BookDTO BookToDTO(Book book) =>
            new BookDTO
            {
                Isbn = book.Isbn,
                AuthorId = book.AuthorId,
                CategoryId = book.CategoryId,
                Title = book.Title,
                Quantity = book.Quantity
            };
        static BookDetailsDTO BookDetailsToDTO(Book book) =>
            new BookDetailsDTO
            {
                Isbn = book.Isbn,
                Title = book.Title,
                Quantity = book.Quantity,
                Author = AuthorToDTO(book.Author),
                Category = CategoryToDTO(book.Category)
            };

        static AuthorDTO AuthorToDTO(Author author) =>
            new AuthorDTO
            {
                AuthorId = author.AuthorId,
                Name = author.Name
            };

        static CategoryDTO CategoryToDTO(Category category) =>
            new CategoryDTO
            {
                CategoryId = category.CategoryId,
                Name = category.Name
            };
    }
}
