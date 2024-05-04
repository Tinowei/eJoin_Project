namespace ApplicationCore.Services;


public static class CustomUtcNow
{
    public static DateTime CurrentTime
    {
        get
        {
            var utcNow = DateTime.UtcNow.AddHours(8);
            return new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute, utcNow.Second, 0);
        }
    }
}