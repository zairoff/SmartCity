using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sport.API.Auth;
using Sport.Infrastructure.Base;
using Sport.Service;
using Sport.Service.Interfaces;
using System.IO;

namespace Sport.API.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
                                                                IWebHostEnvironment hostEnvironment,
                                                                IConfiguration configuration)
        {
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<ISportTypeService, SportTypeService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ITrainerService, TrainerService>();
            services.AddScoped<ITrainerGroupService, TrainerGroupService>();
            services.AddScoped<ITraineeService, TraineeService>();
            services.AddScoped<IPocketService, PocketService>();
            services.AddScoped<ISportGroupService, SportGroupService>();
            services.AddScoped<IVacancyService, VacancyService>();
            services.AddScoped<IApplicantService, ApplicantService>();
            services.AddScoped<IEventParticipantService, EventParticipantService>();
            services.AddScoped<IEventSubscriberService, EventSubscriberService>();
            services.AddScoped<IEventWinnerService, EventWinnerService>();
            services.AddScoped<ISportEventService, SportEventService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();

            var contentRoot = Path.Combine(hostEnvironment.WebRootPath, configuration["AppSettings:LogFolder"]);
            services.AddScoped<ILogger>(x => new FileLogger(contentRoot));

            return services;
        }
    }
}
