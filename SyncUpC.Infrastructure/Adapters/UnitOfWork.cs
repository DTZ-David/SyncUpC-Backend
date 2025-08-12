using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Configuration.Claims;
using SyncUpC.Domain.Ports.Services;

namespace SyncUpC.Infraestructure.Adapters;

public class UnitOfWork : IUnitOfWork
{
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

    public UnitOfWork(IAccountService accountService,
                      IUserService studentService,
                      IClaimService claimsService,
                      IEventService eventService,
                      ICareerService careerService,
                      IFacultyService facultyService,
                      IForumService forumService,
                      IQRService qRService,
                      IEmailService emailService,
                      IAttendanceService attendance)
    {
        AccountService = accountService;
        UserService = studentService;
        ClaimsService = claimsService;
        CareerService = careerService;
        FacultyService = facultyService;
        AttendanceService = attendance;
        EventService = eventService;
        ForumService = forumService;
        QRService = qRService;
        EmailService = emailService;
    }

}
