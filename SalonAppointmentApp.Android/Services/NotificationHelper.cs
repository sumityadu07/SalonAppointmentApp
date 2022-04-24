using SalonAppointmentApp.Droid.Services;
using SalonAppointmentApp.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotificationHelper))]
namespace SalonAppointmentApp.Droid.Services
{
    public class NotificationHelper : INotification
    {
        public void SendMessage(string topic, string title, string body)
        {

        }
    }
}