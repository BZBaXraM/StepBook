namespace Account.API.Services;

/// <summary>
/// Black list service.
/// </summary>
public class BlackListService : IBlackListService
{
    private HashSet<string> BlackList { get; set; } = [];

    /// <summary>
    /// Add token to black list.
    /// </summary>
    /// <param name="token"></param>
    public void AddTokenToBlackList(string token)
    {
        BlackList.Add(token);
    }

    /// <summary>
    /// Check if token is blacklisted.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public bool IsTokenBlackListed(string token)
    {
        return BlackList.Contains(token);
    }
}