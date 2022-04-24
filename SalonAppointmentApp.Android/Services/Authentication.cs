using Firebase;
using Firebase.Auth;
using Java.Util.Concurrent;
using SalonAppointmentApp.Droid;
using SalonAppointmentApp.Services;
using System.Threading.Tasks;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(Authentication))]
namespace SalonAppointmentApp.Droid
{
    public class Authentication : IAuthentication
    {
        const int OTP_TIMEOUT = 30;
        private TaskCompletionSource<bool> _phoneAuthTcs;
        private string _verificationId;
        public Authentication()
        {

        }

        public string CurrentUid() => "7y94m7uMLKbYKzeqSjPKh9wXu8S2";
        public string Phone() => "";

        public bool IsSignIn()
            => false;

        public void SignOut()
            => DialogService.ToastAsync("jjk");

        public Task<bool> VerifyAndUpdateNumber(string code)
        {
            if (!string.IsNullOrWhiteSpace(_verificationId))
            {
            }
            return Task.FromResult(false);
        }

        public Task<bool> VerifyEmail(string email)
        {
            var verify = new TaskCompletionSource<bool>();
            //var action = ActionCodeSettings.NewBuilder()
            //    .SetUrl("")
            //    .SetHandleCodeInApp(true)
            //    .SetAndroidPackageName("com.swibblinc.swibbl", true, "8")
            //    .Build();
            //FirebaseAuth.Instance.SendSignInLinkToEmail(email, action);
            return verify.Task;
        }

        [System.Obsolete]
        public Task<bool> SendOtpCodeAsync(string phoneNumber)
        {
            _phoneAuthTcs = new TaskCompletionSource<bool>();
            return _phoneAuthTcs.Task;
        }
        public Task<bool> ResendOtpCodeAsync(string phoneNumber)
        {
            return _phoneAuthTcs.Task;
        }

        public Task<bool> VerifyOtpCodeAsync(string code)
        {
            if (!string.IsNullOrWhiteSpace(_verificationId))
            {
            }
            return Task.FromResult(false);
        }

        private void OnAuthCompleted(Task task, TaskCompletionSource<bool> tcs)
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                tcs.SetResult(false);
                return;
            }
            _verificationId = null;
            tcs.SetResult(true);
        }
    }
}