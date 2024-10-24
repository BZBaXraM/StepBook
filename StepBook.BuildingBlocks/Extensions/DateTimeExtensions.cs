namespace StepBook.BuildingBlocks.Extensions;

public static class DateTimeExtensions
{
    public static int CalculateAge(this DateTime dateTime)
    {
        var today = DateTime.Today;
        var age = today.Year - dateTime.Year;

        if (dateTime.Date > today.AddYears(-age)) age--;

        return age;
    }
}