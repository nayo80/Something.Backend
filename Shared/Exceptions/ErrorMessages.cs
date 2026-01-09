namespace Shared.Exceptions;

/// <summary>
///     Contains various error messages as enum values
/// </summary>
public enum ErrorMessages
{
    #region User Errors
    UserNotFound,
    UserNotLoggedIn,
    UserNotEnoughPermissions,
    AuthNotPermitted,
    AuthTokenInvalid,
    AuthRefreshTokenInvalid,
    UserIsDeleted,
    AuthPermitUserIsDeleted,
    ExpressionOfInterestFailed,
    ConfirmationFailed,
    AuthorizationFailedCheckCredentials,
    #endregion
    
    #region RestaurantNotFound

    RestaurantNotFound,
    DescriptionShouldBeTranslated,

    #endregion

    #region Role Errors

    RoleDataRequired,
    PermittedEndpointsNotFound,
    PermittedEndpointsIdCantBeNegative,
    
    #endregion

    #region Car

    CarNotFound,

    #endregion

    #region Password Errors

    InvalidPassword,
    PasswordsNotMatches,

    #endregion
    
    #region WorkingHours

    RestaurantWorkingHoursFromHasInvalidFormat,
    RestaurantWorkingHoursToHasInvalidFormat,
    KitchenWorkingHoursFromHasInvalidFormat,
    KitchenWorkingHoursToHasInvalidFormat,
    WorkingHoursFromCannotBeGreaterThanWorkingHoursTo,

    #endregion

    
    
    
    #region UploadImage Error

    WrongImageExtension,
    ImageSizeIsMoreThan5Mb,
    ImageNotUploaded,
    InvalidImageDirectory,
    ImageNotFound,
    #endregion

}