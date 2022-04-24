using SalonAppointmentApp.Models.Salon;
using SalonAppointmentApp.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace SalonAppointmentApp.Pages
{
    public class AppointmentsPageModel : ViewModelBase
    {
        IRepository<Appointment> Repository = DependencyService.Get<IRepository<Appointment>>();

        public AppointmentsPageModel()
        {
            appointments = new ObservableCollection<Appointment>();
            GetAppointments();
            MessagingCenter.Unsubscribe<Appointment, SummaryPageModel>(this, "BookAppointment");
            MessagingCenter.Subscribe<Appointment, SummaryPageModel>(this, "BookAppointment", async (arg, sender) =>
            {
                Appointments.Add(arg);
                await Repository.SaveAppointment(arg);
            });

            MessagingCenter.Subscribe<MenuPageModel>(this, "LoggedOut", async (sender) =>
            {
                await Refresh();
            });
        }

        public ICommand CancelCommand => new Command<Appointment>(CancelOrder);
        public ICommand BookAgainCommand => new Command<Appointment>(BookAgain);
        public ICommand RefreshCommand => new AsyncCommand(Refresh);

        async void GetAppointments()
        {
            if (!AuthService.IsSignIn())
            {
                State = LayoutState.Empty;
                MessagingCenter.Subscribe<LoginBaseModel>(this, "Login", async (sender) =>
                {
                    await Refresh();
                });
            }
            else
            {
                await LoadData();
            }
        }

        async Task Refresh()
        {
            IsBusy = true;
            await LoadData();
            IsBusy = false;
        }

        async Task LoadData()
        {
            State = LayoutState.Empty;
            if (AuthService.IsSignIn())
            {
                Appointments = await Repository.GetCollectionbyUserId("appointments");
                if (Appointments != null)
                    State = LayoutState.None;
            }
        }
        async void CancelOrder(Appointment appointment)
        {
            await DialogService.AlertAsync("Cancel Order", "Are you Sure you want to cancel Order?", "Ok");
            await Repository.CancelAppointment(appointment);
            await DialogService.ToastAsync("Order cancelled,for more info go to Orders page");
        }

        async void Selected(Appointment appointment)
        {
            if (appointment == null)
                return;
            SelectedSalon = null;
            await CoreMethods.PushPageModel<AppointmentDetailPageModel>(appointment, animate: false);
        }

        async void BookAgain(Appointment appointment)
        {
            if (appointment == null)
                return;
            SelectedSalon = null;
            await CoreMethods.PushPageModel<SlotPageModel>(appointment, animate: false);
        }

        private Appointment selectedSalon;
        public Appointment SelectedSalon
        {
            get => selectedSalon;
            set
            {
                SetProperty(ref selectedSalon, value);
                Selected(SelectedSalon);
            }
        }
        private ObservableCollection<Appointment> appointments;
        public ObservableCollection<Appointment> Appointments
        {
            get => appointments;
            set => SetProperty(ref appointments, value);
        }
    }
}
