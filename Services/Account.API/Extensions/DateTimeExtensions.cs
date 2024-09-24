namespace Account.API.Extensions;

/// <summary>
/// The DateTimeExtentions class
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Calculate the age of a user
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static int CalculateAge(this DateTime dateTime)
    {
        var today = DateTime.Today;
        var age = today.Year - dateTime.Year;

        if (dateTime.Date > today.AddYears(-age)) age--;

        return age;
    }
}