using Entities1.DTO;
using Entities1.Exceptions1;
using Entities1.Models;
using Entities1.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ServiceFilter(typeof(LogFilterAttributes))]
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
        public async Task<IActionResult> GetAllBooksAsync( [FromQuery] BookParameters bookParameters)
        {
            var pagedResult = await _manager.BookService.GetAllBooksAsync(bookParameters,false);

            Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.books);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneBookAsync([FromRoute(Name = "id")] int id)
        {
            // Kitap bulunamazsa servis katmanı BookNotFound fırlatır (404).
            var book = await _manager.BookService.GetOneBookByIdAsync(id, false);

            return Ok(book);
        }

        
        
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost]
        public async Task<IActionResult> CreateOneBookAsync([FromBody] BookDtoForInsertion bookDto)
        {
          
            var book= await _manager.BookService.CreateOneBookAsync(bookDto);

            return StatusCode(201, book);
        }

        
        
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneBookAsync([FromRoute(Name = "id")] int id, [FromBody] BookDtoForUpdate bookDto)
        {
           
            await _manager.BookService.UpdateOneBookAsync(id, bookDto, false);

            return Ok(bookDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOneBookAsync([FromRoute(Name = "id")] int id)
        {
            // Silinen kitabı geri döndürmek için önce okunur (bulunamazsa 404).
            var book = await _manager.BookService.GetOneBookByIdAsync(id, false);

            await _manager.BookService.DeleteOneBookAsync(id, true);

            return Ok(book);
        }
    }
}
