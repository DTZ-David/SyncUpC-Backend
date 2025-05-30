
namespace SyncUpC.Domain.Common.Enums;

public enum MessageStatusCode
{
    Succes = 200,
    Create = 201,
    BadRequest = 400,
    Unauthorized = 401,
    NotFound = 404,
    Conflict = 409,
    ServerError = 500,
    FieldRequired = 600,
    DomainCannotBeConverted = 601,
    ActionDeny = 602,
    InvalidDate = 603
}
