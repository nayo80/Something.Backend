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
    AuthorizationFailedCheckCredentials,
    #endregion
    

    #region Role Errors

    RoleDataRequired,
    PermittedEndpointsNotFound,
    PermittedEndpointsIdCantBeNegative,
    
    #endregion

    #region Car

    CarNotFound,

    #endregion
    
    #region Player
    PlayerNotFound,
    #endregion

    #region Password Errors
    InvalidPassword,
    #endregion

}