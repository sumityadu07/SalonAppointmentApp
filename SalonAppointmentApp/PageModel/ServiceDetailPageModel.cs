using SalonAppointmentApp.Models.Salon;
using SalonAppointmentApp.Services;
using Xamarin.Forms;

namespace SalonAppointmentApp.Pages
{
    public class ServiceDetailPageModel : ViewModelBase
    {
        IRepository<Service> repository = DependencyService.Get<IRepository<Service>>();

        public override void Init(object initData)
        {
            var name = initData as string;
            Load(name);
        }

        async void Load(string id)
        {
            Service = await repository.GetDocument("services", id);
        }
        private Service service;
        public Service Service
        {
            get => service;
            set => SetProperty(ref service, value);

        }

    }
}
