using System;

public static class AlertService
{
    public static DateTime GetEarliestDateToProccessAlert(int timeThresholdInSeconds)
    {
        var thirtyPercentIncrement = (int)Math.Ceiling(timeThresholdInSeconds * 0.3);
        timeThresholdInSeconds += thirtyPercentIncrement;

        var now = DateTime.UtcNow;

        return now.AddSeconds(-timeThresholdInSeconds);
    }
}