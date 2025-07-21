using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.User.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.User.Commands.CreateStudent;


public record CreateStudentCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string PhoneNumber,
    string ProfilePhotoUrl,
    string CareerId,
    NotificationPreferencesCommand NotificationPreferences
) : IRequest<ActionResult<Response<StudentDto>>>;

public record NotificationPreferencesCommand(
    NotificationSettingCommand EventReminder,
    NotificationSettingCommand EventUpdate,
    NotificationSettingCommand ForumReply,
    NotificationSettingCommand ForumMention
);

public record NotificationSettingCommand(
    bool Push,
    bool Email,
    bool WhatsApp
);
