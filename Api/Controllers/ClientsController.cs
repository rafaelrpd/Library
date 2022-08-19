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
    public class ClientsController : ControllerBase
    {
        private readonly LIBRARYContext _context;

        public ClientsController(LIBRARYContext context)
        {
            _context = context;
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> GetClients()
        {
          if (_context.Clients == null)
          {
              return NotFound();
          }
            return await _context.Clients
                .Select(x => ClientToDTO(x))
                .ToListAsync();
        }

        // GET: api/Clients/CPF number
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(string id)
        {
            if (_context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Where(c => c.Cpf == id)
                .Include(b => b.BorrowedBooks)
                .Select(c => ClientDetailsToDTO(c))
                .ToListAsync();

            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        // PUT: api/Clients/CPF number
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(string id, ClientDTO clientDTO)
        {
            if (id != clientDTO.Cpf)
            {
                return BadRequest();
            }

            ICollection<BorrowedBook> borrowedBooks = _context.BorrowedBooks.Where(ci => ci.ClientId == clientDTO.Cpf).ToList();
            Client client = new Client()
            {
                Cpf = clientDTO.Cpf,
                Name = clientDTO.Name,
                RegistrationDate = clientDTO.RegistrationDate,
                BorrowedBooks = borrowedBooks
            };
            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // POST: api/Clients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(ClientDTO clientDTO)
        {
            if (_context.Clients == null)
            {
                return Problem("Entity set 'LIBRARYContext.Clients'  is null.");
            }

            
            Client client = new Client()
            {
                Cpf = clientDTO.Cpf,
                Name = clientDTO.Name,
                RegistrationDate = clientDTO.RegistrationDate,
                BorrowedBooks = null!
            };
            _context.Clients.Add(client);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClientExists(client.Cpf))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetClient), new { id = clientDTO.Cpf }, clientDTO);
        }

        // DELETE: api/Clients/CPF number
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(string id)
        {
            if (_context.Clients == null)
            {
                return NotFound();
            }
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientExists(string id)
        {
            return (_context.Clients?.Any(e => e.Cpf == id)).GetValueOrDefault();
        }

        static ClientDTO ClientToDTO(Client client) =>
            new ClientDTO
            {
                Cpf = client.Cpf,
                Name = client.Name,
                RegistrationDate = client.RegistrationDate
            };

        static ClientDetailsDTO ClientDetailsToDTO(Client client) =>
            new ClientDetailsDTO
            {
                Cpf = client.Cpf,
                Name = client.Name,
                RegistrationDate = client.RegistrationDate,
                BorrowedBooks = client.BorrowedBooks.Select(b => BorrowedBookToDTO(b)).ToList()
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
