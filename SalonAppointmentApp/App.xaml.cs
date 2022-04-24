using FreshMvvm;
using Plugin.FirebasePushNotification;
using SalonAppointmentApp.Pages;
using SalonAppointmentApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace SalonAppointmentApp
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            InitializeComponent();
            InitNavigation();
        }

        public void InitNavigation()
        {
            var loginpage = FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
            var LoginStack = new FreshNavigationContainer(loginpage, NavigationStack.LoginNavigationStack);
            var FreshShell = new FreshTabbedNavigationContainer(NavigationStack.MainAppStack);
            FreshShell.On<Android>().DisableSmoothScroll();
            FreshShell.On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            FreshShell.On<Android>().SetIsSwipePagingEnabled(false);
            var menuicon = new FontImageSource() { Glyph = FontAwesome.FontAwesomeIcons.User, FontFamily = "FAR" };
            var apico = new FontImageSource() { Glyph = FontAwesome.FontAwesomeIcons.Clipboard, FontFamily = "FAR" };
            var exploreicon = new FontImageSource() { Glyph = FontAwesome.FontAwesomeIcons.Compass, FontFamily = "FAR" };
            FreshShell.AddTab<MainPageModel>("Home", "home.png", null);
            FreshShell.AddTab<ExplorePageModel>("Explore", null, null).IconImageSource = exploreicon;
            FreshShell.AddTab<AppointmentsPageModel>("Appointments", null, null).IconImageSource = apico;
            FreshShell.AddTab<MenuPageModel>("Profile", null, null).IconImageSource = menuicon;
            FreshShell.SelectedTabColor = Color.MediumPurple;
            FreshShell.UnselectedTabColor = Color.DimGray;
            FreshShell.BarBackgroundColor = Color.White;
            FreshShell.BarTextColor = Color.Black;
            MainPage = LoginStack;
        }

        protected override void OnStart()
        {
            CrossFirebasePushNotification.Current.Subscribe("General");
        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {
        }

    }
}
