using SyncUpC.Domain.Ports.Configuration.Claims;
using SyncUpC.Domain.Ports.Services;

namespace SyncUpC.Domain.Ports;

public interface IUnitOfWork
{
    IAccountService AccountService { get; }
    IUserService UserService { get; }
    IClaimService ClaimsService { get; }
    IAttendanceService AttendanceService { get; }
    IEmailService EmailService { get; }
    IQRService QRService { get; }
    IEventService EventService { get; }
    IFacultyService FacultyService { get; }
    ICareerService CareerService { get; }
    IForumService ForumService { get; }
}
