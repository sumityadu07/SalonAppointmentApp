using SalonAppointmentApp.Models.Salon;
using SalonAppointmentApp.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace SalonAppointmentApp.Pages
{
    public class SlotPageModel : ViewModelBase
    {
        IRepository<Slot> _repository = DependencyService.Get<IRepository<Slot>>();
        public override void Init(object initData)
        {
            Cart = initData as Cart;
            GetDates();
            MessagingCenter.Subscribe<MenuPageModel>(this, "LoggedOut", (sender) =>
            {
                IsVisible = true;
            });
        }

        async void GetDates()
        {
            Dates = await _repository.GetDocument("dates", "datevalue");
            if (!AuthService.IsSignIn())
                IsVisible = true;
        }

        public ICommand GetSlot => new AsyncCommand<string>(GetSlots);
        public ICommand DoneCommand => new AsyncCommand(Done);
        public ICommand LoginCommand => new AsyncCommand(OpenLoginState);

        async Task OpenLoginState()
        {
            MessagingCenter.Subscribe<LoginBaseModel>(this, "Login", (sender) =>
            {
                IsVisible = false;
            });
            await CoreMethods.PushPageModel<LoginDockPageModel>(null, animate: false);
        }

        async Task GetSlots(string date)
        {
            Date = date;
            IsBusy = true;
            if (Date != null)
            {
                Time = new ObservableCollection<Slot>();
                Time.Clear();
                Time = await _repository.GetCollection("slots/");
            }
            IsBusy = false;
        }

        async Task Done()
        {
            if (SelectedSlot != null)
            {
                Cart.Slot = Date + " " + SelectedSlot.Time;
                await CoreMethods.PushPageModel<AddressPageModel>(Cart, animate: false);
            }
            else
                await DialogService.ToastAsync("Please select a time slot to continue");
        }

        private ObservableCollection<Slot> time;
        public ObservableCollection<Slot> Time
        {
            get { return time; }
            set
            {
                SetProperty(ref time, value);
            }
        }
        private string date;
        public string Date
        {
            get { return date; }
            set
            {
                SetProperty(ref date, value);
            }
        }

        private Slot selectedSlot;
        public Slot SelectedSlot
        {
            get { return selectedSlot; }
            set
            {
                SetProperty(ref selectedSlot, value);
            }
        }
        private Slot dates;
        public Slot Dates
        {
            get { return dates; }
            set
            {
                SetProperty(ref dates, value);
            }
        }
    }
}
