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

namespace Sport.API.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<SportType, SportTypeResponseDto>()
                .ForMember(s => s.Sport, o => o.MapFrom(s => s.Name));            

            CreateMap<Position, PositionResponseDto>()
                .ForMember(s => s.Position, o => o.MapFrom(s => s.Name));

            CreateMap<Employee, EmployeeResponseDto>();

            CreateMap<Trainer, TrainerResponseDto>();

            CreateMap<Trainee, TraineeResponseDto>();

            CreateMap<Pocket, PocketResponseDto>()
                .ForMember(p => p.Pocket, o => o.MapFrom(p => p.Name));

            CreateMap<SportGroup, SportGroupResponseDto>();

            CreateMap<Vacancy, VacancyResponseDto>();

            CreateMap<Applicant, ApplicantResponseDto>();

            CreateMap<EventParticipant, EventParticipantResponseDto>();

            CreateMap<EventSubscriber, EventSubscriberResponseDto>();

            CreateMap<EventWinner, EventWinnerResponseDto>();

            CreateMap<TrainerGroup, TrainerGroupResponseDto>();

            CreateMap<SportEvent, SportEventResponseDto>()
                .ForPath(s => s.EventName, o => o.MapFrom(s => s.Name));
        }
    }
}
