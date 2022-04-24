
using SalonAppointmentApp.Services;
using Xamarin.Forms;

namespace SalonAppointmentApp.Models.Salon
{
    public class Appointment : BindableObject, IIdentifiable
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public int Discount { get; set; }
        public int Convenience { get; set; }
        public int SubTotal { get; set; }
        public int Savings { get { return OrderTotal * Discount / 100; } }

        public string Services { get; set; }
        public int OrderTotal { get; set; }
        public int Duration { get; set; }
        public string OrderDate { get; set; }
        public string Slot { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
    }
}
