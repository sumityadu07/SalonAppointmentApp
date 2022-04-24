using SalonAppointmentApp.Models.Salon;
using SalonAppointmentApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SalonAppointmentApp.Pages
{
    public class ViewModelBase : FreshMvvm.FreshBasePageModel, INotifyPropertyChanged
    {
        public IAuthentication AuthService = DependencyService.Resolve<IAuthentication>();

        public ViewModelBase()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsConnected = Connectivity.NetworkAccess == NetworkAccess.Internet;
        }

        ~ViewModelBase()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsConnected = e.NetworkAccess == NetworkAccess.Internet;
            if (!IsConnected)
            {
                var messageOptions = new MessageOptions
                {
                    Foreground = Color.White,
                    Font = Font.SystemFontOfSize(16),
                    Message = "No Internet connection"
                };
                var actionOptions = new SnackBarActionOptions
                {
                    ForegroundColor = Color.White,
                    Padding = 10,
                    Font = Font.SystemFontOfSize(14),
                    Text = "Ok",
                    Action = () => // null by default	
                    {
                        return Task.CompletedTask;
                    }
                };
                var options = new SnackBarOptions
                {
                    MessageOptions = messageOptions,
                    Duration = TimeSpan.MaxValue,
                    BackgroundColor = Color.Default,
                    Actions = new[] { actionOptions }
                };
                await Application.Current.MainPage.DisplaySnackBarAsync(options);
            }
        }

        bool isVisible = false;
        public bool IsVisible
        {
            get => isVisible;
            set => SetProperty(ref isVisible, value);
        }

        Cart cart;
        public Cart Cart
        {
            get => cart;
            set => SetProperty(ref cart, value);
        }

        bool isConnected = false;
        public bool IsConnected
        {
            get => isConnected;
            set => SetProperty(ref isConnected, value);
        }
        bool isBusy = false;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        LayoutState state;
        public LayoutState State
        {
            get => state;
            set => SetProperty(ref state, value);
        }
        string customState;
        public string CustomState
        {
            get => customState;
            set => SetProperty(ref customState, value);
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
           [CallerMemberName] string propertyName = "",
           Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
