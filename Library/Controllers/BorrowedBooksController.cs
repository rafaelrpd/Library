using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.Models;

namespace Library.Controllers
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
        public async Task<ActionResult<IEnumerable<BorrowedBook>>> GetBorrowedBooks()
        {
          if (_context.BorrowedBooks == null)
          {
              return NotFound();
          }
            return await _context.BorrowedBooks.ToListAsync();
        }

        // GET: api/BorrowedBooks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowedBook>> GetBorrowedBook(int id)
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

            return borrowedBook;
        }

        // PUT: api/BorrowedBooks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBorrowedBook(int id, BorrowedBook borrowedBook)
        {
            if (id != borrowedBook.Id)
            {
                return BadRequest();
            }

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
        public async Task<ActionResult<BorrowedBook>> PostBorrowedBook(BorrowedBook borrowedBook)
        {
          if (_context.BorrowedBooks == null)
          {
              return Problem("Entity set 'LIBRARYContext.BorrowedBooks'  is null.");
          }
            _context.BorrowedBooks.Add(borrowedBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBorrowedBook), new { id = borrowedBook.Id }, borrowedBook);
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
    }
}
