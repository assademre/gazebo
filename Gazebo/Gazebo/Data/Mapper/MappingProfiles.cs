using AutoMapper;
using EventOrganizationApp.Models;
using EventOrganizationApp.Models.Enums;

namespace EventOrganizationApp.Data.Dto.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Event, EventDto>()
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => (EventType)src.EventTypeId))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => (CurrencyModel)src.CurrencyId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.GetName(typeof(Status), src.StatusId)));

            CreateMap<EventDto, Event>()
                .ForMember(dest => dest.EventTypeId, opt => opt.MapFrom(src => (int)src.EventId))
                .ForMember(dest => dest.CurrencyId, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<EventsTask, EventTaskDto>()
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => (CurrencyModel)src.CurrencyId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.GetName(typeof(Status), src.StatusId)));

            CreateMap<EventTaskDto, EventsTask>()
                .ForMember(dest => dest.CurrencyId, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
