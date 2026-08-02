namespace Isotainer.Module.Tank.Helpers.Extensions;

public static class TimeSpanExtensions
{
    public static string ToReadableDuration(this TimeSpan timeSpan)
    {
        if (timeSpan <= TimeSpan.Zero)
        {
            return "N/A";
        }

        if (timeSpan.TotalSeconds < 60)
        {
            var seconds = (int)Math.Round(timeSpan.TotalSeconds);
            return seconds == 1 ? "1 second" : $"{seconds} seconds";
        }

        if (timeSpan.TotalMinutes < 60)
        {
            var minutes = (int)Math.Round(timeSpan.TotalMinutes);
            return minutes == 1 ? "1 minute" : $"{minutes} minutes";
        }

        if (timeSpan.TotalHours < 24)
        {
            var hours = (int)Math.Round(timeSpan.TotalHours);
            return hours == 1 ? "1 hour" : $"{hours} hours";
        }

        if (timeSpan.TotalDays < 30)
        {
            var days = (int)Math.Round(timeSpan.TotalDays);
            return days == 1 ? "1 day" : $"{days} days";
        }

        if (timeSpan.TotalDays < 365)
        {
            var months = (int)Math.Floor(timeSpan.TotalDays / 30);
            return months == 1 ? "1 month" : $"{months} months";
        }

        var years = (int)Math.Floor(timeSpan.TotalDays / 365);
        return years == 1 ? "1 year" : $"{years} years";
    }
}