using System.Threading.Tasks;

namespace MeteoHealth.Services
{
    public interface IReminderService
    {
        public Task ScheduleDailyReminder(int hour, int minute);
    }
}
