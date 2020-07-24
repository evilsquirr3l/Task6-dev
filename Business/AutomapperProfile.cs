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
            CreateMap<Book, BookModel>().ReverseMap();
            CreateMap<Card, CardModel>().ReverseMap();

            CreateMap<Reader, ReaderModel>()
                .ForMember(destination => destination.Phone,
                    map => map.MapFrom(source => source.ReaderProfile.Phone))
                .ForMember(destination => destination.Address,
                    map => map.MapFrom(source => source.ReaderProfile.Address));
            
            CreateMap<ReaderModel, Reader>()
                .ForMember(destination => destination.ReaderProfile,
                    map => map.MapFrom(
                        source => new ReaderProfile
                        {
                            ReaderId = source.Id,
                            Phone = source.Phone,
                            Address = source.Address
                        }));
            CreateMap<Book, BookModel>()
                .ForMember(p => p.CardsIds, c => c.MapFrom(card => card.Cards.Select(x => x.CardId)))
                .ReverseMap();

            CreateMap<Card, CardModel>()
                .ForMember(p => p.BooksIds, c => c.MapFrom(card => card.Books.Select(x => x.BookId)))
                .ReverseMap();
        }
    }
}