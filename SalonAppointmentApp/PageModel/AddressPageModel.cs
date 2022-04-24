using SalonAppointmentApp.Models.Salon;
using SalonAppointmentApp.Models.User;
using SalonAppointmentApp.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace SalonAppointmentApp.Pages
{
    class AddressPageModel : ViewModelBase
    {
        IRepository<Address> _repository = DependencyService.Get<IRepository<Address>>();

        public override void Init(object initData)
        {
            Cart = initData as Cart;
            addresses = new ObservableCollection<Address>();
            GetLocations();
        }

        async void GetLocations() => Addresses = await _repository.GetCollectionbyUserId("addresses");
        async void AddAddress(string address) => await _repository.SaveAddress(address);
        public ICommand NewAddressCommand => new AsyncCommand(GoToGeolocationPage);
        public ICommand DoneCommand => new AsyncCommand(Done);

        async Task Done()
        {
            if (SelectedAddress != null)
            {
                Cart.Address = SelectedAddress.Location;
                await CoreMethods.PushPageModel<SummaryPageModel>(Cart, animate: false);
            }
            else
                await DialogService.ToastAsync("Please select a time slot to continue");
        }

        public override void ReverseInit(object returnedData)
        {
            AddAddress(returnedData as string);
            GetLocations();
        }

        async Task GoToGeolocationPage()
        {
            await CoreMethods.PushPageModel<GeoLocationPageModel>(_repository.Phone, animate: false);
        }

        private ObservableCollection<Address> addresses;
        public ObservableCollection<Address> Addresses
        {
            get { return addresses; }
            set => SetProperty(ref addresses, value);
        }

        private Address selectedAddress;
        public Address SelectedAddress
        {
            get => selectedAddress;
            set
            {
                SetProperty(ref selectedAddress, value);
            }
        }
    }
}
