using AutoMapper;
using Sport.API.DTOs.ApplicantDto;
using Sport.API.DTOs.EmployeeDto;
using Sport.API.DTOs.EventParticipantDto;
using Sport.API.DTOs.EventSubscriberDto;
using Sport.API.DTOs.EventWinnerDto;
using Sport.API.DTOs.PocketDto;
using Sport.API.DTOs.PositionDto;
using Sport.API.DTOs.SportEventDto;
using Sport.API.DTOs.SportGroupDto;
using Sport.API.DTOs.SportTypeDto;
using Sport.API.DTOs.TraineeDto;
using Sport.API.DTOs.TrainerDto;
using Sport.API.DTOs.TrainerGroupDto;
using Sport.API.DTOs.VacancyDto;
using Sport.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<SportTypeCreateDto, SportType>()
                .ForMember(s => s.Name, r => r.MapFrom(s => s.Sport));

            CreateMap<SportTypeUpdateDto, SportType>()
                .ForMember(s => s.Name, r => r.MapFrom(s => s.Sport));

            CreateMap<PositionCreateDto, Position>()
                .ForMember(s => s.Name, r => r.MapFrom(s => s.Position));

            CreateMap<PositionUpdateDto, Position>()
                .ForMember(s => s.Name, r => r.MapFrom(s => s.Position));

            CreateMap<EmployeeCreateDto, Employee>();

            CreateMap<EmployeeUpdateDto, Employee>();

            CreateMap<TrainerCreateDto, Trainer>();

            CreateMap<TrainerUpdateDto, Trainer>();

            CreateMap<TraineeCreateDto, Trainee>();

            CreateMap<TraineeUpdateDto, Trainee>();

            CreateMap<PocketCreateDto, Pocket>()
                .ForMember(p => p.Name, r => r.MapFrom(p => p.Pocket));

            CreateMap<PocketUpdateDto, Pocket>();

            CreateMap<PocketUpdateDto, Pocket>();

            CreateMap<SportGroupCreateDto, SportGroup>();

            CreateMap<SportGroupUpdateDto, SportGroup>();

            CreateMap<VacancyCreateDto, Vacancy>();

            CreateMap<VacancyUpdateDto, Vacancy>();

            CreateMap<ApplicantCreateDto, Applicant>();

            CreateMap<EventParticipantCreateDto, EventParticipant>();

            CreateMap<EventSubscriberCreateDto, EventSubscriber>();

            CreateMap<EventWinnerCreateDto, EventWinner>();

            CreateMap<TrainerGroupCreateDto, TrainerGroup>();

            CreateMap<TrainerGroupUpdateDto, TrainerGroup>();

            CreateMap<SportEventUpdateDto, SportEvent>();

            CreateMap<SportEventCreateDto, SportEvent>();
        }
    }
}
