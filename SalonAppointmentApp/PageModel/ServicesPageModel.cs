using SalonAppointmentApp.Models.Salon;
using SalonAppointmentApp.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace SalonAppointmentApp.Pages
{
    public class ServicesPageModel : ViewModelBase
    {
        IRepository<Service> _repository = DependencyService.Get<IRepository<Service>>();

        public override void Init(object initData)
        {
            State = LayoutState.Loading;
            LoadServices(initData.ToString());
            State = LayoutState.None;
        }

        async void LoadServices(string value)
        {
            await Task.Delay(1000);
            services = new ObservableCollection<Service>();
            Services.Clear();
            Services = await _repository.SortItemsbyCategory(value);
        }

        public ICommand DecrementCommand => new Command<Service>(Decrement);
        public ICommand OpenSlotCommand => new AsyncCommand(OpenSlot);
        public ICommand IncrementCommand => new Command<Service>(Increment);
        public ICommand AddServiceCommand => new Command<Service>(CreatePack);

        #region Pack
        void CreatePack(Service service)
        {
            selectedServices = new ObservableCollection<Service>();
            IsVisible = true;
            service.IsVisible = true;
            SelectedServices.Clear();
            SelectedServices.Add(service);
            Sumup();
        }

        void Decrement(Service service)
        {
            if (service.Quantity > 1)
                service.Quantity--;
            else
            {
                service.IsVisible = false;
                SelectedServices.Remove(service);
            }

            Sumup();

        }

        void Increment(Service service)
        {
            service.Quantity++;
            Sumup();
        }
        async Task OpenSlot()
        {
            Cart = new Cart();
            foreach (var p in SelectedServices)
                Cart.Services.Add(p);
            await CoreMethods.PushPageModel<SlotPageModel>(Cart, animate: false);
        }
        void Sumup()
        {
            if (SelectedServices != null)
            {

                var amt = new List<int>();
                var dura = new List<int>();
                foreach (var p in SelectedServices)
                {
                    if (p is Service ser)
                    {
                        amt.Add(ser.Price * ser.Quantity);
                        dura.Add(ser.Duration * ser.Quantity);
                    }
                }
                Amt = amt.Sum();
                Duration = dura.Sum();
                if (Amt == 0)
                    IsVisible = false;
            }
        }

        #endregion

        private ObservableCollection<Service> selectedServices;
        public ObservableCollection<Service> SelectedServices
        {
            get { return selectedServices; }
            set => SetProperty(ref selectedServices, value);

        }

        private string slot = "Pick a time Slot";
        public string Slot
        {
            get { return slot; }
            set
            {
                SetProperty(ref slot, value);
            }
        }
        private ObservableCollection<Service> services;
        public ObservableCollection<Service> Services
        {
            get => services;
            set => SetProperty(ref services, value);

        }
        private int amt;
        public int Amt
        {
            get => amt;
            set
            {
                SetProperty(ref amt, value);

            }
        }
        private int duration;
        public int Duration
        {
            get => duration;
            set
            {
                SetProperty(ref duration, value);
            }
        }
    }
}
