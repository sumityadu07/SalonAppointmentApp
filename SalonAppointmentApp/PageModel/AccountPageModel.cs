using SalonAppointmentApp.Models.User;
using SalonAppointmentApp.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace SalonAppointmentApp.Pages
{
    public class AccountPageModel : LoginBaseModel
    {
        IRepository<UserInfo> Repository = DependencyService.Resolve<IRepository<UserInfo>>();

        private bool isAUpdate;
        private UserInfo user;
        private string newPhone;

        public AccountPageModel()
        {
            Phone = Repository.Phone;
            LoadData();
        }

        async void LoadData()
        {
            User = await Repository.GetUserAsync();
        }

        public ICommand SaveCommand => new AsyncCommand(OnSave);
        public ICommand AddMailCommand => new AsyncCommand(AddEmail);
        public ICommand OpenTabCommand => new Command(() => IsAUpdate = true);
        public ICommand CloseTabCommand => new Command(() => IsAUpdate = false);

        public ICommand UpdateCommand => new AsyncCommand(UpdatePhoneNumber);

        private async Task OnSave()
        {
            if (User.Name == null)
            {
                await Repository.Save(user.Name, user.Email, user.Image, user.Gender);
            }
            await Repository.Update(user.Name, user.Email, user.Image, user.Gender);
        }

        async Task AddEmail()
        {
            if (User.Email != null)
                IsVerified = await AuthService.VerifyEmail(user.Email);
        }

        async Task UpdatePhoneNumber()
        {
            if (codeRequested)
            {
                await VerifyAndUpdateOtp();
                if (IsVerified)
                    IsAUpdate = false;
            }
            else
            {
                await SendOtp();
            }
        }

        public UserInfo User
        {
            get => user;
            set => SetProperty(ref user, value);
        }

        public bool IsAUpdate
        {
            get => isAUpdate;
            set => SetProperty(ref isAUpdate, value);
        }

        public string NewPhone
        {
            get => newPhone;
            set => SetProperty(ref newPhone, value);
        }
    }
}
