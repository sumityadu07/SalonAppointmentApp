using SalonAppointmentApp.Models.Salon;
using SalonAppointmentApp.Services;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace SalonAppointmentApp.Pages
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class AppointmentDetailPageModel : ViewModelBase
    {
        IRepository<Appointment> _repository = DependencyService.Get<IRepository<Appointment>>();

        public AppointmentDetailPageModel()
        {
            Services = new ObservableCollection<Service>();
        }

        async void GetData(string id)
        {
            var _repository = DependencyService.Get<IRepository<Appointment>>();
            Order = await _repository.GetDocument("Orders", id);
            switch (Order.Status)
            {
                case "Cancelled":
                    StatusColor = Color.Red;
                    IsButtonVisible = true;
                    break;
                case "Refund Initiated":
                    StatusColor = Color.Blue;
                    break;
                case "Refunded":
                    StatusColor = Color.Green;
                    break;
                default:
                    StatusColor = Color.WhiteSmoke;
                    break;
            }
        }
        private string id;
        public string Id
        {
            get => id;
            set
            {
                id = value;
                GetData(Id);
            }
        }
        private ObservableCollection<Service> services;
        public ObservableCollection<Service> Services
        {
            get { return services; }
            set => SetProperty(ref services, value);

        }

        private Appointment order;
        public Appointment Order
        {
            get => order;
            set => SetProperty(ref order, value);
        }
        private bool isEnabled;
        public bool IsEnabled
        {
            get => isEnabled;
            set => SetProperty(ref isEnabled, value);
        }
        private bool isErrorVisible;
        public bool IsErrorVisible
        {
            get => isErrorVisible;
            set => SetProperty(ref isErrorVisible, value);
        }

        private Color statusColor;
        public Color StatusColor
        {
            get => statusColor;
            set
            {
                SetProperty(ref statusColor, value);
                GetData(Id);
            }
        }

        private bool isButtonVisible;
        public bool IsButtonVisible
        {
            get => isButtonVisible;
            set => SetProperty(ref isButtonVisible, value);
        }
        public string Comment { get; set; }
        private int savings;
        public int Savings
        {
            get => savings;
            set => SetProperty(ref savings, value);
        }
        private int convenienceCharge;
        public int ConvenienceCharge
        {
            get => convenienceCharge;
            set => SetProperty(ref convenienceCharge, value);
        }
        private int charge;
        public int Charge
        {
            get => charge;
            set => SetProperty(ref charge, value);
        }
        private int total;
        public int Total
        {
            get => total;
            set => SetProperty(ref total, value);
        }
        private string servicename;
        public string Servicename
        {
            get => servicename;
            set
            {
                SetProperty(ref servicename, value);
            }
        }
        private string address;
        public string Address
        {
            get => address;
            set
            {
                SetProperty(ref address, value);
            }
        }
    }
}
