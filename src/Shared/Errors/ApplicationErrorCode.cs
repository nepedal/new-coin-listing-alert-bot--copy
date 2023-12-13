namespace Shared.Errors;

public enum ApplicationErrorCode : short
{
    [Display(Name = "Bad Request")]
    BadRequest = 1000,

    [Display(Name = "Unauthorized")]
    Unauthorized = 1002,

    [Display(Name = "Access denied or user banned")]
    Forbidden = 1003,

    [Display(Name = "Internal Server Error")]
    InternalServerError = 1004,

    [Display(Name = "Email is not verified")]
    GoogleAuthEmailNotVerified = 1005,

    [Display(Name = "Duplicate email address")]
    DuplicateEmailAddress = 1006,

    [Display(Name = "User not registered")]
    UserNotRegistered = 1007,

    [Display(Name = "IdToken is invalid")]
    GoogleAuthIdTokenInvalid = 1008,

    [Display(Name = "Language does not exist")]
    LanguageDoesNotExist = 1009,

    [Display(Name = "You can't leave a meeting you never joined")]
    CannotLeaveMeeting = 1010,

    [Display(Name = "Apple Auth Code Invalid")]
    AppleAuthCodeInvalid = 1011,

    [Display(Name = "Entity Not Found")]
    EntityNotFound = 1012
}
