using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MeteoHealth.Services
{
    public interface IReminderService
    {
        public Task ScheduleDailyReminder(int hour, int minute);
    }
}
