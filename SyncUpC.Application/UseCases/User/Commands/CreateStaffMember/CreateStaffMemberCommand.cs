using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.User.Commands.CreateStudent;
using SyncUpC.Application.UseCases.User.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.User.Commands.CreateStaffMember;

public record CreateStaffMemberCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string PhoneNumber,
    string ProfilePhotoUrl,
    string Profession,
    string Department,
    string Position,
    string FacultyId,
    NotificationPreferencesCommand NotificationPreferences) : IRequest<ActionResult<Response<StaffMemberDto>>>;

