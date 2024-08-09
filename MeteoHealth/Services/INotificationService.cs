using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoHealth.Services
{
    public interface INotificationService
    {
        public void CallDailyReminder(int hour, int minute);
    }
}
