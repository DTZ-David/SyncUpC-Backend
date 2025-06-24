using FluentValidation;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Helpers;
using SyncUpC.Domain.Ports;


namespace SyncUpC.Application.UseCases.User.Students.Commands.CreateStudent;

public class CreateStudentValidation : AbstractValidator<CreateStudentCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateStudentValidation(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(s => s.FirstName)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio");

        RuleFor(s => s.LastName)
            .NotEmpty()
            .WithMessage("El apellido es obligatorio");

        RuleFor(s => s.Email)
            .NotEmpty().WithMessage("El correo es obligatorio")
            .EmailAddress().WithMessage("El correo no es válido")
            .MustAsync(BeUniqueEmailAsync)
            .WithMessage("Este correo ya está registrado");

        RuleFor(s => s.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria")
            .MinimumLength(6)
            .WithMessage("La contraseña debe tener al menos 6 caracteres");

        RuleFor(s => s.PhoneNumber)
            .NotEmpty()
            .WithMessage("El número de teléfono es obligatorio");

        RuleFor(s => s.ProfilePhotoUrl)
            .Must(BeAValidUrl)
            .When(s => !string.IsNullOrWhiteSpace(s.ProfilePhotoUrl))
            .WithMessage("La URL de la foto de perfil no es válida");

        RuleFor(s => s.CareerId)
            .NotEmpty()
            .WithMessage("La carrera es obligatoria")
            .Must(ObjectIdValidation.IsValidObjectId)
            .WithMessage("La carrera no es válida");

        RuleFor(s => s.FacultyId)
            .NotEmpty()
            .WithMessage("La facultad es obligatoria")
            .Must(ObjectIdValidation.IsValidObjectId)
            .WithMessage("La facultad no es válida");

        // Si tienes notificaciones, podrías validar con otro validador personalizado aquí
        // RuleFor(s => s.NotificationPreferences).SetValidator(new NotificationPreferencesValidator());
    }

    private async Task<bool> BeUniqueEmailAsync(string email, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.StudentService.GetStudentByEmail(email);
        return existingUser == null;
    }

    private bool BeAValidUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return true;

        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
