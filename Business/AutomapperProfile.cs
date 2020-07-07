using AutoMapper;
using Business.Models;
using Data.Entities;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Book, BookModel>().ReverseMap();
            CreateMap<Reader, ReaderModel>().ReverseMap();
            CreateMap<Card, CardModel>().ReverseMap();
        }
    }
}