
namespace SyncUpC.Domain.Common.Enums;

public static class MessageCode
{
    #region 1##### - Token / Auth
    public static string InvalidFormatToken => "1A####";
    public static string UnauthorizedToken => "1B####";
    public static string WithoutPermissionsToken => "1C####";
    #endregion

    #region 2##### - General
    public static string ErrorValidations => "2A####";
    public static string ProcessSuccess => "2B####";
    public static string NotFound => "2C####";
    public static string ConflictError => "2D####";
    #endregion

    #region 3##### - Auth / Sesión
    public static string IncorrectCredentials => "3A####";
    public static string InvalidVerificationCode => "3B####";
    public static string SignUpAppimotionPlus => "3C####";
    #endregion

    #region 4##### - User / Student / Validation
    public static string FirstNameRequired => "4A001";
    public static string LastNameRequired => "4A002";
    public static string PhoneNumberRequired => "4A003";
    public static string PasswordTooShort => "4A004";
    public static string InvalidEmail => "4A005";
    public static string MailAlreadyExists => "4A006";
    public static string InvalidUrl => "4A007";
    public static string InvalidCareerId => "4A008";
    public static string InvalidFacultyId => "4A009";
    public static string EmailRequired => "4A010";
    public static string PasswordRequired => "4A011";
    #endregion
}
