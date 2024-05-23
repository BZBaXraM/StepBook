using StepBook.API.Data.Entities;

namespace StepBook.API.Providers;

public interface IRequestUserProvider
{
    UserInfo? GetUserInfo();
}