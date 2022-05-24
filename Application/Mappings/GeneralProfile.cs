using AutoMapper;
using Domain.Entities;

//community
using Application.Features.Communities.Commands.CreateCommunity;
using Application.Features.Communities.Commands.UpdateCommunity;
using Application.Features.Communities.Queries.GetAllCommunities;
using Application.Features.Communities.Queries.GetCommunityById;
using Application.Features.Communities.Queries.GetLoggedInCommunity;

//student
using Application.Features.Students.Commands.CreateStudent;
using Application.Features.Students.Queries.GetAllStudents;
using Application.Features.Students.Commands.UpdateStudent;
using Application.Features.Students.Queries.GetStudentById;
using Application.Features.Students.Queries.GetLoggedInStudent;

//event
using Application.Features.Events.Queries.GetAllEvents;
using Application.Features.Events.Queries.GetEventById;
using Application.Features.Events.Queries.GetEventsByStudentId;
using Application.Features.Events.Commands.CreateEvent;
using Application.Features.Events.Commands.UpdateEvent;

//notification
using Application.Features.Notifications.Commands.CreateNotification;
using Application.Features.Notifications.Queries.GetAllNotifications;
using Application.Features.Notifications.Queries.GetAllNotificationsByCommunityId;

//report
using Application.Features.Reports.Commands.CreateReport;
using Application.Features.Reports.Queries.GetAllReports;

namespace Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<CreateStudentCommand, Student>();
            CreateMap<Student, GetAllStudentsViewModel>().ReverseMap();
            CreateMap<UpdateStudentCommand, Student>();
            CreateMap<GetAllStudentsQuery, GetAllStudentsParameter>();
            CreateMap<Student, GetLoggedInStudentViewModel>().ReverseMap();
            CreateMap<Student, GetStudentByIdViewModel>().ReverseMap();
            CreateMap<Student, GetAllCommunitiesStudentViewModel>();
            CreateMap<Student, GetCommunityByIdStudentViewModel>();
            CreateMap<Student, GetLoggedInCommunityStudentViewModel>();
            CreateMap<Student, GetAllEventsStudentViewModel>();
            CreateMap<Student, GetEventByIdStudentViewModel>();
            CreateMap<Student, GetAllReportsStudentViewModel>();

            CreateMap<CreateCommunityCommand, Community>();
            CreateMap<Community, GetCommunityByIdViewModel>().ReverseMap();
            CreateMap<UpdateCommunityCommand, Community>();
            CreateMap<GetAllCommunitiesQuery, GetAllCommunitiesParameter>();
            CreateMap<Community, GetLoggedInCommunityViewModel>().ReverseMap();
            CreateMap<Community, GetAllCommunitiesViewModel>().ReverseMap();
            CreateMap<Community, GetAllStudentsCommunityViewModel>();
            CreateMap<Community, GetStudentByIdCommunityViewModel>();
            CreateMap<Community, GetLoggedInStudentCommunityViewModel>();
            CreateMap<Community, GetAllEventsCommunityViewModel>();
            CreateMap<Community, GetEventsByStudentIdCommunityViewModel>();
            CreateMap<Community, GetEventByIdCommunityViewModel>();
            CreateMap<Community, GetAllNotificationsCommunityViewModel>();

            CreateMap<Event, GetAllEventsViewModel>().ReverseMap();
            CreateMap<CreateEventCommand, Event>();
            CreateMap<UpdateEventCommand, Event>();
            CreateMap<GetAllEventsQuery, GetAllEventsParameter>();
            CreateMap<GetEventsByStudentIdQuery, GetEventsByStudentIdParameter>();
            CreateMap<Event, GetEventByIdViewModel>();
            CreateMap<Event, GetEventsByStudentIdViewModel>();
            CreateMap<Event, GetAllStudentsEventViewModel>();
            CreateMap<Event, GetLoggedInStudentEventViewModel>();
            CreateMap<Event, GetStudentByIdEventViewModel>();
            CreateMap<Event, GetAllCommunitiesEventViewModel>();
            CreateMap<Event, GetLoggedInCommunityEventViewModel>();
            CreateMap<Event, GetCommunityByIdEventViewModel>();
            CreateMap<Event, GetAllReportsEventViewModel>();

            CreateMap<Notification, GetAllNotificationsViewModel>().ReverseMap();
            CreateMap<CreateNotificationCommand, Notification>();
            CreateMap<GetAllNotificationsQuery, GetAllNotificationsParameter>();
            CreateMap<GetAllNotificationsByCommunityIdQuery, GetAllNotificationsByCommunityIdParameter>();
            CreateMap<Notification, GetAllNotificationsByCommunityIdViewModel>();

            CreateMap<Report, GetAllReportsViewModel>().ReverseMap();
            CreateMap<CreateReportCommand, Report>();
            CreateMap<GetAllReportsQuery, GetAllReportsParameter>();

        }
    }
}
