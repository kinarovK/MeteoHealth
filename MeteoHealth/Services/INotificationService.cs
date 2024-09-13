namespace MeteoHealth.Services
{
    public interface INotificationService
    {
        public void CallDailyReminder(int hour, int minute);
    }
}
