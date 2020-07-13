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
            CreateMap<Card, CardModel>().ReverseMap();
            CreateMap<History, HistoryModel>().ReverseMap();
        }
    }
}