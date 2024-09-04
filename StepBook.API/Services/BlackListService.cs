namespace StepBook.API.Services;

public class BlackListService : IBlackListService
{
    private HashSet<string> BlackList { get; set; } = new();

    public void AddTokenToBlackList(string token)
    {
        BlackList.Add(token);
    }

    public bool IsTokenBlackListed(string token)
    {
        return BlackList.Contains(token);
    }
}