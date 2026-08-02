namespace Isotainer.Core.Api.Extensions;

public static class DateTimeExtensions
{
    public static string ToRelativeTime(this DateTime utcDateTime)
    {
        if (utcDateTime == DateTime.MinValue || 
            utcDateTime.Year == 1 || 
            utcDateTime.Year == 1970)
        {
            return "Never";
        }
        
        var delta = DateTime.UtcNow - utcDateTime;

        if (delta.TotalSeconds < 0)
        {
            return "Just now";
        }

        if (delta.TotalSeconds < 60)
        {
            return Math.Abs(delta.TotalSeconds - 1) < 0.0001 ? "One second ago" : $"{delta.Seconds} seconds ago";
        }
        
        if (delta.TotalMinutes < 60)
        {
            return delta.TotalMinutes < 2 ? "A minute ago" : $"{delta.Minutes} minutes ago";
        }
        
        if (delta.TotalHours < 24)
        {
            return delta.TotalHours < 2 ? "An hour ago" : $"{delta.Hours} hours ago";
        }
        
        if (delta.TotalDays < 7)
        {
            return delta.TotalDays < 2 ? "Yesterday" : $"{delta.Days} days ago";
        }

        if (delta.TotalDays < 30)
        {
            var weeks = (int)Math.Floor(delta.TotalDays / 7);
            return weeks == 1 ? "A week ago" : $"{weeks} weeks ago";
        }

        if (delta.TotalDays < 365)
        {
            var months = (int)Math.Floor(delta.TotalDays / 30);
            return months == 1 ? "A month ago" : $"{months} months ago";
        }

        var years = (int)Math.Floor(delta.TotalDays / 365);
        return years == 1 ? "A year ago" : $"{years} years ago";
    }
}