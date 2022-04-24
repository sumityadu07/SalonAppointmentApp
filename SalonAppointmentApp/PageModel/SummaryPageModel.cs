using AllInOneSDK;
using SalonAppointmentApp.Models.Salon;
using SalonAppointmentApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SalonAppointmentApp.Pages
{
    public class SummaryPageModel : ViewModelBase, PaymentCallback
    {
        IRepository<Appointment> repository = DependencyService.Get<IRepository<Appointment>>();

        public override void Init(object initData)
        {
            Cart = initData as Cart;
            SumUp();
        }

        void SumUp()
        {
            var amt = new List<int>();
            var dura = new List<int>();
            foreach (var p in Cart.Services)
            {
                if (p is Service ser)
                {
                    amt.Add(ser.Price * ser.Quantity);
                    dura.Add(ser.Duration * ser.Quantity);
                }
            }

            Total = amt.Sum();
            if (Total == 0)
                Back();
            Duration = dura.Sum();
            Savings = Total * Discount / 100;
            SubTotal = Total - Savings;
            AllTotal = SubTotal + ConvenienceFee;

            Appointment = new Appointment
            {
                OrderTotal = AllTotal,
                Services = string.Join(",", Cart.Services),
                OrderId = Guid.NewGuid().ToString().Substring(0, 12),
                Address = Cart.Address,
                Duration = Duration
            };

            MessagingCenter.Subscribe<SummaryPage>(this, "Close", (sender) =>
            {
                Back();
            });
        }

        public ICommand DecrementCommand => new Command<Service>(Decrement);
        public ICommand IncrementCommand => new Command<Service>(Increment);
        public ICommand OrderCommand => new Command(MakePayment);

        void Decrement(Service service)
        {
            if (service.Quantity > 1)
                service.Quantity--;
            else
                Cart.Services.Remove(service);
            SumUp();
        }

        void Increment(Service service)
        {
            service.Quantity++;
            SumUp();
        }

        void Back()
        {
            //CoreMethods.RemoveFromNavigation<ServicesPageModel>();
            CoreMethods.PopToRoot(true);
        }
        async void MakePayment()
        {
            AllInOnePlugin.startTransaction(Appointment.OrderId,
 "LagQoK20596609643874", Guid.NewGuid().ToString(), Appointment.OrderTotal.ToString(), "https://securegw.paytm.in/theia/paytmCallback?ORDER_ID=ktm00220022@Hl", false, false, this);
            MessagingCenter.Send(this, "BookAppointment", Appointment);
        }


        public async void success(Dictionary<string, object> dictionary)
        {
            await repository.SaveAppointment(Appointment);
            AllInOnePlugin.DestroyInstance();
        }

        public async void error(string errorMessage)
        {
            await DialogService.AlertAsync("Payment", errorMessage, "Ok");
            AllInOnePlugin.DestroyInstance();
        }

        public Appointment Appointment { get; set; }

        int savings;
        public int Savings
        {
            get => savings;
            set => SetProperty(ref savings, value);
        }
        int subTotal;
        public int SubTotal
        {
            get => subTotal;
            set => SetProperty(ref subTotal, value);
        }
        int duration;
        public int Duration
        {
            get => duration;
            set => SetProperty(ref duration, value);
        }

        int allTotal;
        public int AllTotal
        {
            get => allTotal;
            set => SetProperty(ref allTotal, value);
        }

        int total;
        public int Total
        {
            get => total;
            set => SetProperty(ref total, value);
        }

        int discount = 15;
        public int Discount
        {
            get => discount;
            set => SetProperty(ref discount, value);
        }

        int convenienceFee = 15;
        public int ConvenienceFee
        {
            get => convenienceFee;
            set => SetProperty(ref convenienceFee, value);
        }
    }
}
