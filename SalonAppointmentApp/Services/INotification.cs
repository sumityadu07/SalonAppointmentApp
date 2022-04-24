namespace SalonAppointmentApp.Services
{
    public interface INotification
    {
        void SendMessage(string topic, string title, string body);
    }
}
