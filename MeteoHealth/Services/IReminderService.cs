using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoHealth.Services
{
    public interface IReminderService
    {
        public void ScheduleDailyReminder(int hour, int minute);
    }
}
