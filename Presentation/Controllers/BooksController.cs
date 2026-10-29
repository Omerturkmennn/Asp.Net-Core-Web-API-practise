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
        public IActionResult GetAllBooks()
        {
            var books = _manager.BookService.GetAllBooks(false);

            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            // Kitap bulunamazsa servis katmanı BookNotFound fırlatır (404).
            var book = _manager.BookService.GetOneBookById(id, false);

            return Ok(book);
        }

        [HttpPost]
        public IActionResult CreateOneBook([FromBody] BookDtoForInsertion bookDto)
        {
            if (bookDto is null)
                return BadRequest();

            if (!ModelState.IsValid) 
            {
                return UnprocessableEntity(ModelState);
            }

            var book=_manager.BookService.CreateOneBook(bookDto);

            return StatusCode(201, book);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] BookDtoForUpdate bookDto)
        {
            if (bookDto is null)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            if (bookDto.Id != id)
                return BadRequest("The id in the route does not match the id in the body.");

            _manager.BookService.UpdateOneBook(id, bookDto, false);

            return Ok(bookDto);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            // Silinen kitabı geri döndürmek için önce okunur (bulunamazsa 404).
            var book = _manager.BookService.GetOneBookById(id, false);

            _manager.BookService.DeleteOneBook(id, true);

            return Ok(book);
        }
    }
}
