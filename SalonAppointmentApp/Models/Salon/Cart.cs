using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace SalonAppointmentApp.Models.Salon
{
    public class Cart : BindableObject
    {
        public Cart()
        {
            Services = new ObservableCollection<Service>();
        }

        ObservableCollection<Service> services;
        public ObservableCollection<Service> Services
        {
            get => services;
            set
            {
                services = value;
                OnPropertyChanged("Services");
            }
        }

        public string Address { get; set; }
        public string Slot { get; set; }

    }
}
