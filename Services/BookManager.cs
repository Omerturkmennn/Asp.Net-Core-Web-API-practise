using AutoMapper;
using Entities1.DTO;
using Entities1.Exceptions1;
using Entities1.Models;
using Entities1.RequestFeatures;
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

        public async Task<BookDto> CreateOneBookAsync(BookDtoForInsertion bookDto)
        {
            var entity = _mapper.Map<Book>(bookDto);
            _manager.Book.CreateOneBook(entity);
            await _manager.SaveAsync();
            return _mapper.Map<BookDto>(entity);
        }

        public async Task DeleteOneBookAsync(int id, bool trackChanges)
        {
            var entity = await _manager.Book.GetOneBookByIdAsync(id, trackChanges)
                ?? throw new BookNotFound(id);

            _manager.Book.DeleteOneBook(entity);
            await _manager.SaveAsync();
        }

        public async Task<(IEnumerable<BookDto> books, MetaData metaData)> GetAllBooksAsync(BookParameters bookParameters,bool trackChanges)
        {
            var booksWithMetadata = await _manager.Book.GetAllBooksAsync(bookParameters,trackChanges);
            
            var booksDto= _mapper.Map<IEnumerable<BookDto>>(booksWithMetadata);
            
            return (booksDto, booksWithMetadata.MetaData);
        }

        public async Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges)
        {
            var book = await _manager.Book.GetOneBookByIdAsync(id, trackChanges)
                ?? throw new BookNotFound(id);
            return _mapper.Map<BookDto>(book);
        }

        public async  Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDto,bool trackChanges)
        {
            var entity = await _manager.Book.GetOneBookByIdAsync(id,trackChanges)
                ?? throw new BookNotFound(id);

            
            _mapper.Map(bookDto, entity);

            _manager.Book.Update(entity);
            await _manager.SaveAsync();
        }

        
    }
}
