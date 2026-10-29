using Entities1.DTO;
using Entities1.Exceptions1;
using Entities1.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooksAsync()
        {
            var books = await _manager.BookService.GetAllBooksAsync(false);

            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneBookAsync([FromRoute(Name = "id")] int id)
        {
            // Kitap bulunamazsa servis katmanı BookNotFound fırlatır (404).
            var book = await _manager.BookService.GetOneBookByIdAsync(id, false);

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOneBookAsync([FromBody] BookDtoForInsertion bookDto)
        {
            if (bookDto is null)
                return BadRequest();

            if (!ModelState.IsValid) 
            {
                return UnprocessableEntity(ModelState);
            }

            var book= await _manager.BookService.CreateOneBookAsync(bookDto);

            return StatusCode(201, book);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneBookAsync([FromRoute(Name = "id")] int id, [FromBody] BookDtoForUpdate bookDto)
        {
            if (bookDto is null)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            if (bookDto.Id != id)
                return BadRequest("The id in the route does not match the id in the body.");

            await _manager.BookService.UpdateOneBookAsync(id, bookDto, false);

            return Ok(bookDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOneBookAsync([FromRoute(Name = "id")] int id)
        {
            // Silinen kitabı geri döndürmek için önce okunur (bulunamazsa 404).
            var book = _manager.BookService.GetOneBookByIdAsync(id, false);

            await _manager.BookService.DeleteOneBookAsync(id, true);

            return Ok(book);
        }
    }
}
