
namespace SyncUpC.Domain.Common.Enums;


public static class MessageCode
{
    #region 1#####
    public static string InvalidFormatToken => "1A####";
    public static string UnauthorizedToken => "1B####";
    public static string WithoutPermissionsToken => "1C####";
    #endregion

    #region 2#####
    public static string ErrorValidations => "2A####";
    public static string ProcessSuccess => "2B####";
    public static string NotFound => "2C####";
    public static string ConflictError => "2D####";

    #endregion

    #region 3#####
    public static string IncorrectCredentials => "3A####";
    public static string InvalidVerificationCode => "3B####";
    public static string SignUpAppimotionPlus => "3C####";
    #endregion
}
