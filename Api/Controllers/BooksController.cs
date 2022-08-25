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
    public class BooksController : ControllerBase
    {
        private readonly LIBRARYContext _context;

        public BooksController(LIBRARYContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
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
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(string id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Where(i => i.Isbn == id)
                .Include(a => a.Author)
                .Include(c => c.Category)
                .Include(bb => bb.BorrowedBooks)
                .Select(b => BookDetailsToDTO(b))
                .ToListAsync();

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(string id, BookDTO bookDTO)
        {
            if (id != bookDTO.Isbn)
            {
                return BadRequest();
            }

            Author author = _context.Authors.Where(ai => ai.AuthorId == bookDTO.AuthorId).First();
            Category category = _context.Categories.Where(ai => ai.CategoryId == bookDTO.CategoryId).First();
            ICollection<BorrowedBook> borrowedBooks = _context.BorrowedBooks.Where(bi => bi.BookId == bookDTO.Isbn).ToList();
            Book book = new Book()
            {
                Isbn = bookDTO.Isbn,
                AuthorId = bookDTO.AuthorId,
                CategoryId = bookDTO.CategoryId,
                Title = bookDTO.Title,
                Quantity = bookDTO.Quantity,
                Author = author,
                Category = category,
                BorrowedBooks = borrowedBooks
            };

            _context.Entry(book).State = EntityState.Modified;

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
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(BookDTO bookDTO)
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

            return CreatedAtAction(nameof(GetBook), new { id = bookDTO.Isbn }, bookDTO);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
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
                AuthorId = book.AuthorId,
                CategoryId = book.CategoryId,
                Title = book.Title,
                Quantity = book.Quantity,
                Author = AuthorToDTO(book.Author),
                Category = CategoryToDTO(book.Category),
                BorrowedBooks = book.BorrowedBooks.Select(bb => BorrowedBookToDTO(bb)).ToList()
            };

        static AuthorDTO AuthorToDTO(Author author) =>
            new AuthorDTO
            {
                AuthorId = author.AuthorId,
                Name = author.Name,
                RegistrationDate = author.RegistrationDate
            };

        static CategoryDTO CategoryToDTO(Category category) =>
            new CategoryDTO
            {
                CategoryId = category.CategoryId,
                Name = category.Name
            };

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
    }
}
