using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.User.Students.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.User;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.User.Students.Commands.CreateStudent;

public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, ActionResult<Response<StudentDto>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;


    public CreateStudentCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;

    }

    public async Task<ActionResult<Response<StudentDto>>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        var faculty = new Faculty("685ad08eb1262f0763410dc5", "Ingeniería");
      
        var career = new Career("665f4d2d1c9d440001fcf001","Ingeniería de Sistemas", faculty);

        var notificationPreferences = new NotificationPreferences
        (
            eventReminders: new NotificationSetting
            (
                push : request.NotificationPreferences.EventReminder.Push,
                email : request.NotificationPreferences.EventReminder.Email,
                whatsApp : request.NotificationPreferences.EventReminder.WhatsApp
            ),
            eventUpdates : new NotificationSetting
            (
                push : request.NotificationPreferences.EventUpdate.Push,
                email : request.NotificationPreferences.EventUpdate.Email,
                whatsApp : request.NotificationPreferences.EventUpdate.WhatsApp
            ),
            forumReplies : new NotificationSetting
            (
                push : request.NotificationPreferences.ForumReply.Push,
                email : request.NotificationPreferences.ForumReply.Email,
                whatsApp : request.NotificationPreferences.ForumReply.WhatsApp
            ),
            forumMentions : new NotificationSetting
            (
                push : request.NotificationPreferences.ForumMention.Push,
                email : request.NotificationPreferences.ForumMention.Email,
                whatsApp : request.NotificationPreferences.ForumMention.WhatsApp
            )
        );

        var student = new Student(
            email: request.Email,
            password: request.Password,
            firstName: request.FirstName,
            lastName: request.LastName,
            phoneNumber: request.PhoneNumber,
            profilePhotoUrl: request.ProfilePhotoUrl,
            career: career,
            isActive: true,
            notificationPreferences: notificationPreferences
        );

        await _unitOfWork.StudentService.CreateStudentAsync(student);

        var facultyDto = new FacultyDto(Name: faculty.Name);
        var careerDto = new CareerDto(Name: career.Name, facultyDto);

        var studentDto = new StudentDto(
            Email: student.Email,
            Password: student.Password,
            FirstName: student.Name,
            LastName: student.LastName,
            PhoneNumber: student.PhoneNumber,
            ProfilePhotoUrl: student.ProfilePicture,
            Career: careerDto
        );

        var response = new Response<StudentDto>((int)MessageStatusCode.Create, studentDto);
        return new CreatedResult(string.Empty, response);
    }

}
