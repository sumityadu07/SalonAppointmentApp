using SalonAppointmentApp.Models.User;
using SalonAppointmentApp.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace SalonAppointmentApp.Pages
{
    public class MenuPageModel : LoginBaseModel
    {
        private UserInfo user;
        private string phone;

        public MenuPageModel()
        {
            if (!AuthService.IsSignIn())
            {
                MessagingCenter.Subscribe<LoginBaseModel>(this, "Login", (sender) =>
                {
                    LoadData();
                });
            }
            else
                LoadData();

        }

        async void LoadData()
        {
            State = LayoutState.Loading;
            await Task.Delay(1000);
            State = LayoutState.None;
            if (AuthService.IsSignIn())
            {
                Phone = AuthService.Phone();
                var repository = DependencyService.Get<IRepository<UserInfo>>();
                User = await repository.GetUserAsync();
            }


        }

        public ICommand EditProfileCommand => new AsyncCommand(EditProfileAsync);
        public ICommand AboutCommand => new AsyncCommand(GoToAboutPage);
        public ICommand SignOutCommand => new Command(OnSignOut);
        public ICommand LoginCommand => new AsyncCommand(async () => await CoreMethods.PushPageModel<LoginDockPageModel>(null, true, false));

        async void OnSignOut()
        {
            State = LayoutState.Loading;
            await Task.Delay(1000);
            AuthService.SignOut();
            State = LayoutState.None;
            Phone = null;
            MessagingCenter.Send(this, "LoggedOut");
        }

        async Task EditProfileAsync()
           => await CoreMethods.PushPageModel<AccountPageModel>(animate: false);
        async Task GoToAboutPage()
           => await CoreMethods.PushPageModel<AppDetailPageModel>(animate: false);

        public UserInfo User
        {
            get => user;
            set => SetProperty(ref user, value);
        }
        public string Phone
        {
            get => phone;
            set => SetProperty(ref phone, value);
        }
    }
}
