using System.Linq;
using AutoMapper;
using Business.Models;
using Data.Entities;
using System.Linq;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Book, BookModel>()
                .ForMember(p => p.CardsIds, c => c.MapFrom(card => card.Cards.Select(x => x.CardId)))
                .ReverseMap();

            CreateMap<Card, CardModel>()
                .ForMember(p => p.BooksIds, c => c.MapFrom(card => card.Books.Select(x => x.BookId)))
                .ReverseMap();
        }
    }
}