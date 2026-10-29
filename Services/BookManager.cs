using AutoMapper;
using Entities1.DTO;
using Entities1.Exceptions1;
using Entities1.Models;
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

        public BookDto CreateOneBook(BookDtoForInsertion bookDto)
        {
            var entity=_mapper.Map<Book>(bookDto);
            _manager.Book.CreateOneBook(entity);
            _manager.Save();
            return _mapper.Map<BookDto>(entity);
        }

        public void DeleteOneBook(int id, bool trackChanges)
        {
            var entity = GetOneBookByIdAndCheckExists(id, trackChanges);

            _manager.Book.DeleteOneBook(entity);
            _manager.Save();
        }

        public IEnumerable<BookDto> GetAllBooks(bool trackChanges)
        {
            var books = _manager.Book.GetAllBooks(trackChanges);
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public BookDto GetOneBookById(int id, bool trackChanges)
        {
            var book = GetOneBookByIdAndCheckExists(id, trackChanges);
            return _mapper.Map<BookDto>(book);
        }

        public void UpdateOneBook(int id, BookDtoForUpdate bookDto,bool trackChanges)
        {
            var entity = GetOneBookByIdAndCheckExists(id, trackChanges);

            //entity.Title = book.Title;
            //entity.Price = book.Price;
            // DTO -> mevcut entity üzerine map edilir (yeni nesne oluşturulmaz).
            // Böylece EF Core'un takip ettiği nesne ile çakışma yaşanmaz.
            _mapper.Map(bookDto, entity);

            _manager.Book.UpdateOneBook(entity);
            _manager.Save();
        }

        private Book GetOneBookByIdAndCheckExists(int id, bool trackChanges)
        {
            var entity = _manager.Book.GetOneBookById(id, trackChanges);

            if (entity is null)
            {
                _logger.LogInfo($"The book with id: {id} could not found");
                throw new BookNotFound(id);
            }

            return entity;
        }
    }
}
