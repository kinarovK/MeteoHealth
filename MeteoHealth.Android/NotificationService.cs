using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using MeteoHealth.Services;
[assembly: Dependency(typeof(MeteoHealth.Droid.NotificationService))]
namespace MeteoHealth.Droid
{
    internal class NotificationService : INotificationService
    {
        public void CallDailyReminder(int hour, int minute)
        {
            var notificationTime = new TimeSpan(hour, minute, 0);
            var now = DateTime.Now;
            var firstTriggerTime = new DateTime(now.Year, now.Month, now.Day, notificationTime.Hours, notificationTime.Minutes, notificationTime.Seconds);
            
            if(firstTriggerTime < now)
            {
                firstTriggerTime = firstTriggerTime.AddDays(1);
            }

            var alarmIntent  = new Intent(Android.App.Application.Context, typeof(AlarmReceiver));
            alarmIntent.PutExtra("title", "Reminder");
            alarmIntent.PutExtra("message", "Don't forget to complete today's survey!");

            var pendingIntent = PendingIntent.GetBroadcast(Android.App.Application.Context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);
            var alarmManager = (AlarmManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.AlarmService);

            alarmManager.SetRepeating(AlarmType.RtcWakeup, (long)firstTriggerTime.ToUniversalTime().Subtract(DateTime.UnixEpoch).TotalMilliseconds,
                AlarmManager.IntervalDay, pendingIntent);
        }

        public void CancelReminder()
        {
            var alarmIntent = new Intent(Android.App.Application.Context, typeof(AlarmReceiver));
            var pendingIntent = PendingIntent.GetBroadcast(Android.App.Application.Context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);

            var alarmManager = (AlarmManager)Android.App.Application.Context.GetSystemService(Context.AlarmService);
            alarmManager.Cancel(pendingIntent);
        }
    }
}