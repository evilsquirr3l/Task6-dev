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
                            Id = source.Id,
                            Phone = source.Phone,
                            Address = source.Address
                        }));
        }
    }
}