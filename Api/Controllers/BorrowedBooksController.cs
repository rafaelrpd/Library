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
        [HttpGet]
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

        // GET: api/BorrowedBooks/8886713611511
        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowedBook>> GetBorrowedBook(string id)
        {
          if (_context.BorrowedBooks == null)
          {
              return NotFound();
          }
            var borrowedBook = await _context.BorrowedBooks
                .Where(i => i.Book.Isbn == id)
                .Include(b => b.Book)
                .Include(c => c.Client)
                .Select(bb => BorrowedBookDetailsToDTO(bb))
                .ToListAsync();

            if (borrowedBook == null)
            {
                return NotFound();
            }

            return Ok(borrowedBook);
        }

        // PUT: api/BorrowedBooks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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

        // POST: api/BorrowedBooks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BorrowedBook>> PostBorrowedBook(BorrowedBookDTO borrowedBookDTO)
        {
            if (_context.BorrowedBooks == null)
            {
                return Problem("Entity set 'LIBRARYContext.BorrowedBooks'  is null.");
            }

            Book book = _context.Books.Where(bi => bi.Isbn == borrowedBookDTO.BookId).First();
            Client client = _context.Clients.Where(ci => ci.Cpf == borrowedBookDTO.ClientId).First();
            BorrowedBook borrowedBook = new BorrowedBook()
            {
                ClientId = borrowedBookDTO.ClientId,
                BookId = borrowedBookDTO.BookId,
                BorrowedDate = borrowedBookDTO.BorrowedDate,
                LimitDate = borrowedBookDTO.LimitDate,
                ReturnedDate = borrowedBookDTO.ReturnedDate,
                Book = book,
                Client = client
            };

            _context.BorrowedBooks.Add(borrowedBook);
            await _context.SaveChangesAsync();

            borrowedBookDTO.Id = borrowedBook.Id;
            return CreatedAtAction(nameof(GetBorrowedBook), new { id = borrowedBookDTO.Id }, borrowedBookDTO);
        }

        // DELETE: api/BorrowedBooks/5
        [HttpDelete("{id}")]
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
                Name = client.Name,
                RegistrationDate = client.RegistrationDate
            };
    }
}
