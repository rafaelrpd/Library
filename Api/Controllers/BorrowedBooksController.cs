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
    public class BorrowedBooksController : ControllerBase
    {
        private readonly LIBRARYContext _context;

        public BorrowedBooksController(LIBRARYContext context)
        {
            _context = context;
        }

        // GET: api/BorrowedBooks
        /// <summary>
        /// List all borrowed books with id, clientId, bookId, borrowedDate, limitDate and returnedDate.
        /// </summary>
        /// <returns>List all borrowed books with id, clientId, bookId, borrowedDate, limitDate and returnedDate.</returns>
        /// <remarks>
        /// Instructions: Just send a GET request to URI /api/borrowedbooks.
        /// </remarks>
        /// <response code="200">Returns all borrowed books with simple information.</response>
        /// <response code="404">Borrowed books list not found.</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BorrowedBookDTO>>> GetBorrowedBooks()
        {
            if (_context.BorrowedBooks == null)
            {
                return NotFound();
            }
            return await _context.BorrowedBooks
                .Select(b => BorrowedBookToDTO(b))
                .ToListAsync();
        }

        // GET: api/BorrowedBooks/1
        /// <summary>
        /// Return a specific transaction by ID with a borrowed transaction with full information.
        /// </summary>
        /// <returns>Return a specific transaction by ID with a borrowed transaction with full information.</returns>
        /// <remarks>
        /// Instructions: Just send a GET request to URI /api/borrowedbooks/{id}, where ID is an INT.
        ///     Sample request:
        ///     
        ///         GET /api/borrowedbooks/1
        /// </remarks>
        /// <param name="id" example="1"></param>
        /// <response code="200">Return a specific transaction by ID with a borrowed transaction with full information.</response>
        /// <response code="404">Book not found.</response>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BorrowedBookDetailsDTO>> GetBorrowedBook(int id)
        {
            if (_context.BorrowedBooks == null)
            {
                return NotFound();
            }
            BorrowedBookDetailsDTO borrowedBook = await _context.BorrowedBooks
                .Where(i => i.Id == id)
                .Include(b => b.Book)
                .Include(c => c.Client)
                .Select(bb => BorrowedBookDetailsToDTO(bb))
                .FirstOrDefaultAsync();
            if (borrowedBook == null)
            {
                return NotFound();
            }
            return borrowedBook;
        }

        // PUT: api/BorrowedBooks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Change all info of a borrowed book transaction.
        /// </summary>
        /// <param name="id" example="1"></param>
        /// <param name="borrowedBookDTO"></param>
        /// <returns>Change all info of a borrowed book transaction.</returns>
        /// <remarks>
        /// Instructions: Send a PUT request to URI /api/borrowedbooks/{id}, where ID is an INT with the following body as JSON.
        ///     Sample request:
        ///     
        ///         PUT /api/borrowedbooks/1
        ///         {
        ///             "id": 1,
        ///             "clientId": "11122233345",
        ///             "bookId": "1234567890123",
        ///             "borrowedDate": "2022-08-29T00:00:00",
        ///             "limitDate": "2022-08-29T00:00:00",
        ///             "returnedDate": "2022-08-29T00:00:00"
        ///         }
        /// </remarks>
        /// <response code="204">Changes done correctly.</response>
        /// <response code="400">URI ID not equal as informed in JSON body.</response>
        /// <response code="404">Borrowed book transaction not found.</response>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutBorrowedBook(int id, BorrowedBookDTO borrowedBookDTO)
        {
            if (id != borrowedBookDTO.Id)
            {
                return BadRequest();
            }

            Book book = _context.Books.Where(bi => bi.Isbn == borrowedBookDTO.BookId).First();
            Client client = _context.Clients.Where(ci => ci.Cpf == borrowedBookDTO.ClientId).First();
            BorrowedBook borrowedBook = new BorrowedBook()
            {
                Id = borrowedBookDTO.Id,
                ClientId = borrowedBookDTO.ClientId,
                BookId = borrowedBookDTO.BookId,
                BorrowedDate = borrowedBookDTO.BorrowedDate,
                LimitDate = borrowedBookDTO.LimitDate,
                ReturnedDate = borrowedBookDTO.ReturnedDate,
                Book = book,
                Client = client
            };

            _context.Entry(borrowedBook).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BorrowedBookExists(id))
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

        // POST: api/BorrowedBooks/lend
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create a new borrowed book trasaction.
        /// </summary>
        /// <param name="borrowedBookLendDTO"></param>
        /// <returns>Create a new borrowed book trasaction.</returns>
        /// <remarks>
        /// Instructions: Send a POST request to URI /api/borrowedbooks/lend with the following body as JSON.
        ///     Sample request:
        ///     
        ///     POST /api/borrowedbooks/lend
        ///         {
        ///             "clientId": "11122233345",
        ///             "bookId": "1234567890123"
        ///         }
        /// </remarks>
        /// <response code="204">New borrowed book transaction created successfully.</response>
        /// <response code="400">Book not available to lend or book not found.</response>
        /// <response code="404">Entity set 'LIBRARYContext.BorrowedBooks' is null.</response>
        [HttpPost("lend")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PostBorrowedBookLend(BorrowedBookLendDTO borrowedBookLendDTO)
        {
            if (_context.BorrowedBooks == null)
            {
                return Problem("Entity set 'LIBRARYContext.BorrowedBooks'  is null.");
            }

            if (!CanLendBook(borrowedBookLendDTO.BookId))
            {
                return BadRequest();
            }

            Book book = _context.Books.Where(bi => bi.Isbn == borrowedBookLendDTO.BookId).First();
            if (book == null)
            {
                return BadRequest();
            }

            book.Quantity -= 1;

            Client client = _context.Clients.Where(ci => ci.Cpf == borrowedBookLendDTO.ClientId).First();
            BorrowedBook borrowedBook = new BorrowedBook()
            {
                ClientId = borrowedBookLendDTO.ClientId,
                BookId = borrowedBookLendDTO.BookId,
                Book = book,
                Client = client
            };

            _context.BorrowedBooks.Add(borrowedBook);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/BorrowedBooks/return
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Return a borrowed book.
        /// </summary>
        /// <param name="borrowedBookReturnDTO"></param>
        /// <returns>Return a borrowed book.</returns>
        /// <remarks>
        /// Instructions: Send a POST request to URI /api/borrowedbooks/return with the following body as JSON.
        ///     Sample request:
        ///     
        ///     POST /api/borrowedbooks/return
        ///         {
        ///             "id": 1
        ///         }
        /// </remarks>
        /// <response code="204">Return a borrowed book.</response>
        /// <response code="400">Book not available to return.</response>
        /// <response code="404">Entity set 'LIBRARYContext.BorrowedBooks' is null.</response>
        [HttpPost("return")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PostBorrowedBookReturn(BorrowedBookReturnDTO borrowedBookReturnDTO)
        {
            if (_context.BorrowedBooks == null)
            {
                return Problem("Entity set 'LIBRARYContext.BorrowedBooks'  is null.");
            }

            BorrowedBook borrowedBook = _context.BorrowedBooks.First(bbi => bbi.Id.Equals(borrowedBookReturnDTO.Id));
            if (borrowedBook.ReturnedDate == null)
            {
                borrowedBook.ReturnedDate = DateTime.UtcNow;
                Book book = _context.Books.Single(bi => bi.Isbn.Equals(borrowedBook.BookId));
                book.Quantity += 1;
                await _context.SaveChangesAsync();
            }
                        
            return NoContent();
        }

        // DELETE: api/BorrowedBooks/5
        /// <summary>
        /// Delete a specific borrowed book transaction by ID.
        /// </summary>
        /// <param name="id" example="1"></param>
        /// <returns>Delete a specific borrowed book transaction by ID.</returns>
        /// <remarks>
        /// Instructions: Just send a DELETE request to URI /api/borrowedbooks/{id}, where ID is an INT.
        ///     Sample request:
        ///     
        ///         DELETE /api/borrowedbooks/1007
        /// </remarks>
        /// <response code="204">Delete done successfully.</response>
        /// <response code="404">Borrowed book transaction not found.</response>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBorrowedBook(int id)
        {
            if (_context.BorrowedBooks == null)
            {
                return NotFound();
            }
            var borrowedBook = await _context.BorrowedBooks.FindAsync(id);
            if (borrowedBook == null)
            {
                return NotFound();
            }

            _context.BorrowedBooks.Remove(borrowedBook);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BorrowedBookExists(int id)
        {
            return (_context.BorrowedBooks?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        static BorrowedBookDTO BorrowedBookToDTO(BorrowedBook borrowedBook) =>
            new BorrowedBookDTO
            {
                Id = borrowedBook.Id,
                ClientId = borrowedBook.ClientId,
                BookId = borrowedBook.BookId,
                BorrowedDate = borrowedBook.BorrowedDate,
                LimitDate = borrowedBook.LimitDate,
                ReturnedDate = borrowedBook.ReturnedDate
            };

        static BorrowedBookDetailsDTO BorrowedBookDetailsToDTO(BorrowedBook borrowedBook) =>
            new BorrowedBookDetailsDTO
            {
                Id = borrowedBook.Id,
                ClientId = borrowedBook.ClientId,
                BookId = borrowedBook.BookId,
                BorrowedDate = borrowedBook.BorrowedDate,
                LimitDate = borrowedBook.LimitDate,
                ReturnedDate = borrowedBook.ReturnedDate,
                Book = BookToDTO(borrowedBook.Book),
                Client = ClientToDTO(borrowedBook.Client)
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

        static ClientDTO ClientToDTO(Client client) =>
            new ClientDTO
            {
                Cpf = client.Cpf,
                Name = client.Name
            };

        bool CanLendBook(string isbn)
        {
            var quantity = _context.Books.Where(b => b.Isbn.Equals(isbn)).Select(bq => bq.Quantity).First();
            return (quantity > 0);
        }
    }
}
