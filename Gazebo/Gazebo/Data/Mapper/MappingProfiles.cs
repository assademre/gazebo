﻿using AutoMapper;
using EventOrganizationApp.Models;
using EventOrganizationApp.Models.Enums;
using Gazebo.Data.Dto;
using Gazebo.Models;
using Profile = AutoMapper.Profile;

namespace EventOrganizationApp.Data.Dto.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Event, EventDto>()
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => (EventType)src.EventTypeId))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => (CurrencyModel)src.CurrencyId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (Status)src.StatusId));

            CreateMap<EventDto, Event>()
                .ForMember(dest => dest.EventTypeId, opt => opt.MapFrom(src => (int)Enum.Parse(typeof(EventType), src.EventType)))
                .ForMember(dest => dest.CurrencyId, opt => opt.MapFrom(src => (int)Enum.Parse(typeof(CurrencyModel), src.Currency)))
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (int)Enum.Parse(typeof(Status), src.Status)));

            CreateMap<EventsTask, EventTaskDto>()
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => (CurrencyModel)src.CurrencyId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.GetName(typeof(Status), src.StatusId)));

            CreateMap<EventTaskDto, EventsTask>()
                .ForMember(dest => dest.CurrencyId, opt => opt.MapFrom(src => (int)Enum.Parse(typeof(CurrencyModel), src.Currency)))
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (int)Enum.Parse(typeof(Status), src.Status)));

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username));

            CreateMap<UserDto, User>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username));

            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.CommentOwnerName, opt => opt.Ignore());

            CreateMap<CommentDto, Comment>()
                .ForMember(dest => dest.CommentOwnerId, opt => opt.Ignore());

        }
    }
}
