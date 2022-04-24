using SalonAppointmentApp.Models;
using System.Collections.ObjectModel;

namespace SalonAppointmentApp.Pages
{
    public class AppDetailPageModel : ViewModelBase
    {
        //private readonly FirebaseClient _repository = new("https://swibbl-store-default-rtdb.asia-southeast1.firebasedatabase.app/");

        public AppDetailPageModel()
        {
            Privacies = new ObservableCollection<PrivacyPolicy>();
            Load();
        }

        async void Load()
        {
            //AboutUs = await _repository.Child("aboutus").OnceSingleAsync<AboutUs>();
            // Privacies.Clear();
            // _repository.Child("privacypolicy").AsObservable<PrivacyPolicy>().Subscribe((ad) => Privacies.Add(ad.Object));
        }

        public AboutUs aboutUs;
        public AboutUs AboutUs
        {
            get => aboutUs;
            set => SetProperty(ref aboutUs, value);
        }

        private ObservableCollection<PrivacyPolicy> privacies;
        public ObservableCollection<PrivacyPolicy> Privacies
        {
            get => privacies;
            set => SetProperty(ref privacies, value);
        }
    }
}
