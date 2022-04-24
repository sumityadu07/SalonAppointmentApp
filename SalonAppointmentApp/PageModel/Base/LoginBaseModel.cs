using SalonAppointmentApp.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace SalonAppointmentApp.Pages
{
    public class LoginBaseModel : ViewModelBase
    {
        #region Fields
        private string phone;
        private string code;
        private string error;
        private bool codeSent;
        private string buttonText = "Send Code";
        public bool isVerified;
        public bool codeRequested;
        IAuthentication AuthService = DependencyService.Resolve<IAuthentication>();
        #endregion


        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            if (AuthService.IsSignIn())
                CoreMethods.SwitchOutRootNavigation(NavigationStack.MainAppStack);
        }

        public ICommand NextCommand => new AsyncCommand(MainLoginAction);
        public ICommand SkipCommand => new Command(OnSkip);
        public ICommand PostLoginCommand => new AsyncCommand(OnPostLogin);

        void OnSkip()
        {
            State = LayoutState.Loading;
            CoreMethods.SwitchOutRootNavigation(NavigationStack.MainAppStack);
        }

        async Task MainLoginAction()
        {
            if (codeRequested)
            {
                await VerifyOtp();
                if (IsVerified)
                    CoreMethods.SwitchOutRootNavigation(NavigationStack.MainAppStack);
            }
            else
            {
                await SendOtp();
            }
        }

        async Task OnPostLogin()
        {
            if (codeRequested)
            {
                await VerifyOtp();
                if (IsVerified)
                {
                    CoreMethods.RemoveFromNavigation();
                    MessagingCenter.Send(this, "Login");
                }
            }
            else
            {
                await SendOtp();
            }
        }

        public async Task SendOtp()
        {
            if (!string.IsNullOrEmpty(phone) && phone.Length == 10)
            {
                IsBusy = true;
                CodeSent = await AuthService.SendOtpCodeAsync(Phone);
                IsBusy = false;
                if (!CodeSent)
                    await DialogService.ToastAsync("Something went wrong");
                else
                {
                    codeRequested = true;
                    ButtonText = "Login";
                }
            }
            else
            {
                await DialogService.ToastAsync("Check the number entered");
            }
        }

        public async Task ResendOtp()
        {
            if (!string.IsNullOrEmpty(phone) && phone.Length == 10)
            {
                IsVisible = false;
                IsBusy = true;
                CodeSent = await AuthService.ResendOtpCodeAsync(Phone);
                IsBusy = false;
                if (!CodeSent)
                    await DialogService.ToastAsync("Something went wrong");
                else
                {
                    codeRequested = true;
                    ButtonText = "Login";
                }
            }
            else
            {
                await DialogService.ToastAsync("Check the number entered");
            }
        }

        public async Task VerifyOtp()
        {
            if (!string.IsNullOrEmpty(Code) && Code.Length == 6)
            {
                IsBusy = true;
                IsVerified = await AuthService.VerifyOtpCodeAsync(Code);
                codeRequested = false;
                IsBusy = false;
                Phone = null;
                Code = null;
            }
            else
            {
                await DialogService.ToastAsync("Check the number entered");
            }
        }
        public async Task VerifyAndUpdateOtp()
        {
            if (!string.IsNullOrEmpty(Code) && Code.Length == 6)
            {
                IsBusy = true;
                IsVerified = await AuthService.VerifyAndUpdateNumber(Code);
                codeRequested = false;
                IsBusy = false;
                Phone = null;
                Code = null;
            }
            else
            {
                await DialogService.ToastAsync("Check the number entered");
            }
        }

        #region Properties
        public string Phone
        {
            get => phone;
            set => SetProperty(ref phone, value);
        }

        public string Code
        {
            get => code;
            set => SetProperty(ref code, value);
        }

        public bool CodeSent
        {
            get => codeSent;
            set => SetProperty(ref codeSent, value);
        }
        public string Error
        {
            get => error;
            set => SetProperty(ref error, value);
        }
        public bool IsVerified
        {
            get => isVerified;
            set => SetProperty(ref isVerified, value);
        }
        public string ButtonText
        {
            get => buttonText;
            set => SetProperty(ref buttonText, value);
        }

        #endregion
    }
}