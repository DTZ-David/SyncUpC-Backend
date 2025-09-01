using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Configuration.Claims;
using SyncUpC.Domain.Ports.Services;

namespace SyncUpC.Infraestructure.Adapters;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(IAccountService accountService, IClaimService claimsService, IEventService eventService, IUserService userService, IQRService qRService, IEmailService emailService, IFacultyService facultyService, ICareerService careerService, IAttendanceService attendanceService, IForumService forumService, ISpaceService spaceService, ICampusService campusService, IEventCategoryService eventCategoryService, IEventTypeService eventTypeService, IRegistrationService registrationService)
    {
        AccountService = accountService;
        ClaimsService = claimsService;
        EventService = eventService;
        UserService = userService;
        QRService = qRService;
        EmailService = emailService;
        FacultyService = facultyService;
        CareerService = careerService;
        AttendanceService = attendanceService;
        ForumService = forumService;
        SpaceService = spaceService;
        CampusService = campusService;
        EventCategoryService = eventCategoryService;
        EventTypeService = eventTypeService;
        RegistrationService = registrationService;
    }

    public IAccountService AccountService { get; }
    public IClaimService ClaimsService { get; }
    public IEventService EventService { get; }
    public IUserService UserService { get; }
    public IQRService QRService { get; }
    public IEmailService EmailService { get; }
    public IFacultyService FacultyService { get; }
    public ICareerService CareerService { get; }
    public IAttendanceService AttendanceService { get; }
    public IForumService ForumService { get; }
    public IRegistrationService RegistrationService { get; }
    public ISpaceService SpaceService { get; }
    public ICampusService CampusService { get; }
    public IEventCategoryService EventCategoryService { get; }
    public IEventTypeService EventTypeService { get; }

}
