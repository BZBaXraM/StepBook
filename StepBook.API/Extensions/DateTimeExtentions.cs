namespace StepBook.API.Extensions;

public static class DateTimeExtentions
{
    public static int CalculateAge(this DateTime dateTime)
    {
        var today = DateTime.Today;
        var age = today.Year - dateTime.Year;

        if (dateTime.Date > today.AddYears(-age)) age--;

        return age;
    }
}