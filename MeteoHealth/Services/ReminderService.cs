using SQLite_Database_service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeteoHealth.Services
{
    internal class ReminderService : IReminderService
    {
        private readonly IMeteoHealthRepository meteoHealthRepository;
        private readonly INotificationService notificationService;

        public ReminderService(IMeteoHealthRepository meteoHealthRepository, INotificationService notificationService) 
        {
            this.meteoHealthRepository = meteoHealthRepository;
            this.notificationService = notificationService;
        }

        public void CallDailyReminder(int hour, int minute)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsTodayHealthStateCheckedAsync()
        {
            var todayHealthState = await meteoHealthRepository.GetHealthStatesAsync();

            var today = todayHealthState.FirstOrDefault(x => x.Date == DateTime.Today.ToString());
            if (today  == null)
            {
                return false;
            }
            return true;
        }

        public async Task ScheduleDailyReminder(int hour, int minute)
        {
            if (!await IsTodayHealthStateCheckedAsync())
            {
                notificationService.CallDailyReminder(hour, minute);
            }
        }
    }
}
