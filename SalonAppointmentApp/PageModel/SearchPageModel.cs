
using SalonAppointmentApp.Models.Salon;
using SalonAppointmentApp.Services;
using System;
using Xamarin.Forms;

namespace SalonAppointmentApp.Pages
{
    class SearchPageModel : ViewModelBase
    {
        IRepository<Service> itemrepo = DependencyService.Get<IRepository<Service>>();
        public Type SelectedItemNavigationTarget { get; set; }

        //protected override async void OnQueryChanged(string oldValue, string newValue)
        //{
        //    base.OnQueryChanged(oldValue, newValue);

        //    if (string.IsNullOrWhiteSpace(newValue))
        //    {
        //        ItemsSource = null;
        //    }
        //    else
        //    {
        //        var items = await itemrepo.GetCollection("items");
        //        var salons = await itemrepo.GetCollection("salons");
        //        var source = items.Concat(salons);
        //        ItemsSource = source
        //                       .Where(s => s.Id.ToLower().Contains(newValue.ToLower()))
        //                       .ToList();
        //    }
        //}
        //protected override async void OnItemSelected(object args)
        //{
        //    var item = args as Product;
        //    base.OnItemSelected(item);

        //    var route = $"{nameof(ItemPage)}?{nameof(ItemPageModel.Id)}={item.Id}";
        //    await Shell.Current.GoToAsync(route);
        //}
        //string GetNavigationTarget()
        //{
        //    //return (Shell.Current as AppShell).Routes.FirstOrDefault(route => route.Value.Equals(SelectedItemNavigationTarget)).Key;
        //}
    }
}
