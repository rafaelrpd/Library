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
        /// <summary>
        /// List all clients with CPF and name.
        /// </summary>
        /// <returns>List all clientes with CPF and name.</returns>
        /// <remarks>
        /// Instructions: Just send a GET request to URI /api/clients.
        /// </remarks>
        /// <response code="200">Returns all clients with simple information.</response>
        /// <response code="404">Client list not found.</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        // GET: api/Clients/33399966622
        /// <summary>
        /// Return a specific client with full information.
        /// </summary>
        /// <returns>Return a specific client with full information.</returns>
        /// <remarks>
        /// Instructions: Just send a GET request to URI /api/clients/{id}, where ID is an CPF as INT(11).
        ///     Sample request:
        ///     
        ///         GET /api/books/33399966622
        /// </remarks>
        /// <param name="id" example="33399966622"></param>
        /// <response code="200">Return a specific client with full information.</response>
        /// <response code="404">Client not found.</response>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClientDetailsDTO>> GetClient(string id)
        {
            if (_context.Clients == null)
            {
                return NotFound();
            }

            ClientDetailsDTO client = await _context.Clients
                .Where(c => c.Cpf == id)
                .Include(b => b.BorrowedBooks)
                .Select(c => ClientDetailsToDTO(c))
                .FirstOrDefaultAsync();

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        // PUT: api/Clients/33399966622
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Change all info of a specific client.
        /// </summary>
        /// <param name="id" example="33399966622"></param>
        /// <param name="clientDTO"></param>
        /// <returns>Change all information from a specific client.</returns>
        /// <remarks>
        /// Instructions: Send a PUT request to URI /api/clients/{id}, where ID is an CPF as INT(11) with the following body as JSON.
        ///     Sample request:
        ///     
        ///         PUT /api/books/33399966622
        ///         {
        ///             "cpf": 33399966622,
        ///             "name": "New name after put"
        ///         }
        /// </remarks>
        /// <response code="204">Changes done correctly.</response>
        /// <response code="400">URI ID not equal as informed in JSON body.</response>
        /// <response code="404">Client not found.</response>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutClient(string id, ClientDTO clientDTO)
        {
            if (id != clientDTO.Cpf)
            {
                return BadRequest();
            }

            Client clientContext = await _context.Clients.FirstOrDefaultAsync(ci => ci.Cpf == id);
            if (clientContext == null)
            {
                return NotFound();
            }
            clientContext.Cpf = clientDTO.Cpf;
            clientContext.Name = clientDTO.Name;

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
        /// <summary>
        /// Create a new client.
        /// </summary>
        /// <param name="clientDTO"></param>
        /// <returns>Create a new client.</returns>
        /// <remarks>
        /// Instructions: Send a POST request to URI /api/clients with the following body as JSON.
        ///     Sample request:
        ///     
        ///     POST /api/clients
        ///         {
        ///             "cpf": 8886713611511,
        ///             "name": "3"
        ///         }
        /// </remarks>
        /// <response code="204">New client created successfully</response>
        /// <response code="404">Entity set 'LIBRARYContext.Clients' is null.</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PostClient(ClientDTO clientDTO)
        {
            if (_context.Clients == null)
            {
                return Problem("Entity set 'LIBRARYContext.Clients'  is null.");
            }

            
            Client client = new Client()
            {
                Cpf = clientDTO.Cpf,
                Name = clientDTO.Name,
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

            return NoContent();
        }

        // DELETE: api/Clients/33399966622
        /// <summary>
        /// Delete a specific client.
        /// </summary>
        /// <param name="id" example="33399966622"></param>
        /// <returns>Delete a specific client by CPF.</returns>
        /// <remarks>
        /// Instructions: Just send a DELETE request to URI /api/clients/{id}, where ID is an CPF as INT(11).
        ///     Sample request:
        ///     
        ///         DELETE /api/clients/33399966622
        /// </remarks>
        /// <response code="204">Delete done successfully.</response>
        /// <response code="404">Client not found.</response>
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
                Name = client.Name
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
