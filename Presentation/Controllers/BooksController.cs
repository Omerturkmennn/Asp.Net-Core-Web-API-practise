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

        [HttpGet("{id}")]
        public IActionResult GetOneBook(int id)
        {

            var book = _manager.BookService.GetOneBookById(id, false);

            if (book == null)
               throw new BookNotFound(id);

            return Ok(book);
        }

        [HttpPost]

        public IActionResult CreateOneBook(Book book)
        {
            if (book == null)
                return BadRequest();

            _manager.BookService.CreateOneBook(book);

            return Ok(book);



        }

        [HttpPut("{id}")]

        public IActionResult UpdateOneBook(BookDtoForUpdate bookDto, int id)
        {

            _manager.BookService.UpdateOneBook(id, bookDto, true);

            return Ok(bookDto);
        }

        [HttpDelete("{id}")]

        public IActionResult DeleteOneBook(int id)
        {
            var book = _manager.BookService.GetOneBookById(id, false);

            if (book == null)
                throw new BookNotFound(id);

            _manager.BookService.DeleteOneBook(id, true);

            return Ok(book);
        }
    }
}
