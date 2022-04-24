using SalonAppointmentApp.Pages;
using SalonAppointmentApp.Services;

namespace SalonAppointmentApp.Models.Salon
{
    public class Category : ViewModelBase, IIdentifiable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Imgurl { get; set; }
        private bool isExpanded;

        public bool IsExpanded
        {
            get => isExpanded;
            set => SetProperty(ref isExpanded, value);
        }
        private string expandericon = FontAwesome.FontAwesomeIcons.ChevronDown;
        public string ExpanderIcon
        {
            get { return expandericon; }
            set => SetProperty(ref expandericon, value);
        }

        private string expanderbutton = "OR";
        public string ExpanderbuttonFont
        {
            get { return expanderbutton; }
            set => SetProperty(ref expanderbutton, value);
        }
    }
}
