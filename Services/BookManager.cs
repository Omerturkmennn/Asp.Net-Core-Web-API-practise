using AutoMapper;
using Entities1.DTO;
using Entities1.Models;
using Microsoft.Extensions.Logging;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public BookManager(IRepositoryManager manager,ILoggerService logger,IMapper mapper)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
        }
        
        
        
        public Book CreateOneBook(Book book)
        {
          

           _manager.Book.CreateOneBook(book);
           _manager.Save();
            return book;
        }

        public void DeleteOneBook(int id, bool trackChanges)
        {
            var entity=_manager.Book.GetOneBookById(id,trackChanges);

            if (entity == null)
            {
                string message = $"The book with id: {id} could not found";
                _logger.LogInfo(message);
                throw new Exception(message);
            }

            _manager.Book.DeleteOneBook(entity);
            _manager.Save();
            
        }

        public IEnumerable<Book> GetAllBooks(bool trackChanges)
        {
            return _manager.Book.GetAllBooks(trackChanges);
        }

        public Book GetOneBookById(int id, bool trackChanges)
        {
            return _manager.Book.GetOneBookById(id, trackChanges);  
        }

        public void UpdateOneBook(int id, BookDtoForUpdate bookDto,bool trackChanges)
        {
            var entity = _manager.Book.GetOneBookById(id, trackChanges);

            if (entity == null)
            {
                string message = $"Book with id: {id} could not found ";
               
                _logger.LogInfo(message);
                throw new Exception(message);
            }



            //entity.Title = book.Title;
            //entity.Price = book.Price;
            entity = _mapper.Map<Book>(bookDto);

            _manager.Book.Update(entity);
            _manager.Save();
        }
    }
}
