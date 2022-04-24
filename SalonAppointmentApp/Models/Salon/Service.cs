using SalonAppointmentApp.Services;
using Xamarin.Forms;

namespace SalonAppointmentApp.Models.Salon
{
    public class Service : BindableObject, IIdentifiable
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string ImgUrl { get; set; }

        bool isVisible;
        public bool IsVisible
        {
            get => isVisible;
            set
            {
                isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }

        int quantity = 1;
        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        public int Duration { get; set; }
    }
}
