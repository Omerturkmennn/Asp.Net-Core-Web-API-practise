using AutoMapper;
using Entities1.DTO;
using Entities1.Models;

namespace WebApplication3.Utilities.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookDtoForUpdate, Book>();
        }
    }
}
