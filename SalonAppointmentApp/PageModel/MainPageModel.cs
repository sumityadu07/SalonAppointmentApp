using SalonAppointmentApp.Models.Marketing;
using SalonAppointmentApp.Models.Salon;
using SalonAppointmentApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace SalonAppointmentApp.Pages
{
    public class MainPageModel : ViewModelBase
    {
        IRepository<Category> _repository = DependencyService.Get<IRepository<Category>>();

        public string Woman { get; set; }
        public string Man { get; set; }
        public MainPageModel()
        {
            Load();
            Woman = "https://firebasestorage.googleapis.com/v0/b/swibbl-store.appspot.com/o/WomanServices%2Fwoman-facial.jpg?alt=media&token=a41a79e9-b784-42c4-9304-7a2f8f1b9701";
            Man = "https://firebasestorage.googleapis.com/v0/b/swibbl-store.appspot.com/o/ManServices%2Fbeard.jpg?alt=media&token=f07eed76-6310-4e49-a933-e613c374c3cd";
        }

        async void Load()
        {
            State = LayoutState.Loading;
            IsBusy = true;

            await LoadCarousel();

            await LoadAds();

            IsBusy = false;
            State = LayoutState.None;
        }

        async Task LoadAds()
        {
            IRepository<Ad> _repository = DependencyService.Get<IRepository<Ad>>();
            Ads = new ObservableCollection<Ad>();
            Ads.Clear();
            Ads = await _repository.GetCollection("ads");
        }

        async Task LoadCarousel()
        {
            CarouselAds = new ObservableCollection<Category>();
            CarouselAds.Clear();
            CarouselAds = await _repository.GetCollection("carousel");

            Device.StartTimer(TimeSpan.FromSeconds(2), () =>
            {
                Position = (Position + 1) % CarouselAds.Count;
                return true;
            });
        }

        public ICommand OpenSearchCommand => new AsyncCommand(async () =>
            await CoreMethods.PushPageModel<SearchPageModel>(null, animate: false));
        public ICommand SelectedServiceCommand => new AsyncCommand<string>(SelectedService);
        public ICommand RefreshCommand => new Command(Load);
        public ICommand ManServicesCommand => new AsyncCommand(async () =>
            await CoreMethods.PushPageModelWithNewNavigation<ServicesPageModel>("man", false));
        public ICommand WomandServicesCommand => new AsyncCommand(async () =>
            await CoreMethods.PushPageModelWithNewNavigation<ServicesPageModel>("woman", false));

        async Task SelectedService(string id)
            => await CoreMethods.PushPageModelWithNewNavigation<ServiceDetailPageModel>(id);


        private ObservableCollection<Ad> ads;
        public ObservableCollection<Ad> Ads
        {
            get => ads;
            set => SetProperty(ref ads, value);
        }

        private ObservableCollection<Category> carouselAds;
        public ObservableCollection<Category> CarouselAds
        {
            get => carouselAds;
            set => SetProperty(ref carouselAds, value);
        }

        private int position;
        public int Position
        {
            get { return position; }
            set => SetProperty(ref position, value);
        }
    }
}