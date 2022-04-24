using SalonAppointmentApp.Services;

namespace SalonAppointmentApp.Models.Salon
{
    public class Slot : IIdentifiable
    {
        public string Id { get; set; }
        public string Date { get; set; }
        public string Date1 { get; set; }
        public string Date2 { get; set; }
        public string Date3 { get; set; }
        public string Date4 { get; set; }
        public string Time { get; set; }
    }
}
