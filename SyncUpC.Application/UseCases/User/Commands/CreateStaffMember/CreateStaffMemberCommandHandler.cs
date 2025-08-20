using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.User.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.User;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.User.Commands.CreateStaffMember
{
    public class CreateStaffMemberCommandHandler : IRequestHandler<CreateStaffMemberCommand, ActionResult<Response<StaffMemberDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateStaffMemberCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ActionResult<Response<StaffMemberDto>>> Handle(CreateStaffMemberCommand request, CancellationToken cancellationToken)
        {

            var faculty = await _unitOfWork.FacultyService.GetFacultyById(request.FacultyId);


            var notificationPreferences = new NotificationPreferences(
               eventReminders: new NotificationSetting
               (
                   push: request.NotificationPreferences.EventReminder.Push,
                   email: request.NotificationPreferences.EventReminder.Email,
                   whatsApp: request.NotificationPreferences.EventReminder.WhatsApp
               ),
               eventUpdates: new NotificationSetting
               (
                   push: request.NotificationPreferences.EventUpdate.Push,
                   email: request.NotificationPreferences.EventUpdate.Email,
                   whatsApp: request.NotificationPreferences.EventUpdate.WhatsApp
               ),
               forumReplies: new NotificationSetting
               (
                   push: request.NotificationPreferences.ForumReply.Push,
                   email: request.NotificationPreferences.ForumReply.Email,
                   whatsApp: request.NotificationPreferences.ForumReply.WhatsApp
               ),
               forumMentions: new NotificationSetting
               (
                   push: request.NotificationPreferences.ForumMention.Push,
                   email: request.NotificationPreferences.ForumMention.Email,
                   whatsApp: request.NotificationPreferences.ForumMention.WhatsApp
               )
           );

            var staffMember = new StaffMember(
            email: request.Email,
            password: request.Password,
            firstName: request.FirstName,
            lastName: request.LastName,
            phoneNumber: request.PhoneNumber,
            profilePhotoUrl: request.ProfilePhotoUrl,
            profession: request.Profession,
            department: request.Department,
            position: request.Position,
            isActive: true,
            faculty: faculty,
            notificationPreferences: notificationPreferences
             );

            await _unitOfWork.UserService.CreateUserAsync(staffMember);

            var facacultyDto = new FacultyDto(Name: faculty.Name);

            var studentDto = new StaffMemberDto(
                  Email: staffMember.Email,
                  Password: staffMember.Password,
                  FirstName: staffMember.Name,
                  LastName: staffMember.LastName,
                  PhoneNumber: staffMember.PhoneNumber,
                  ProfilePhotoUrl: staffMember.ProfilePicture,
                  Profession: staffMember.Profession,
                  Department: staffMember.Department,
                  Position: staffMember.Position,
                  Faculty: facacultyDto
             );

            var response = new Response<StaffMemberDto>((int)MessageStatusCode.Create, studentDto);
            return new CreatedResult(string.Empty, response);
        }
    }
}
