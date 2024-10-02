namespace AuthMiddleware.Jwt;

/// <summary>
/// The black list service
/// </summary>
public interface IBlackListService
{
    /// <summary>
    /// Check if a token is blacklisted
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    bool IsTokenBlackListed(string token);

    /// <summary>
    /// Add a token to the black list
    /// </summary>
    /// <param name="token"></param>
    void AddTokenToBlackList(string token);
}