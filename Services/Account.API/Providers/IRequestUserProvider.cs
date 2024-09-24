using Account.API.Shared;

namespace Account.API.Providers;

/// <summary>
/// Request user provider
/// </summary>
public interface IRequestUserProvider
{
    /// <summary>
    /// Get user info
    /// </summary>
    /// <returns></returns>
    UserInfo? GetUserInfo();
}