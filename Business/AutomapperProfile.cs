using System.Linq;
using AutoMapper;
using Business.Models;
using Data.Entities;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Book, BookModel>()
                .ForMember(bm => bm.CardsIds, opt => opt.MapFrom(card => card.Cards.Select(h => h.CardId)))
                .ReverseMap();
        }
    }
}