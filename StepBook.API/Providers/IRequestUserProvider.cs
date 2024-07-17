using StepBook.API.Data.Configs;

namespace StepBook.API.Providers;

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