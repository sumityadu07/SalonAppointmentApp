using SalonAppointmentApp.Services;

namespace SalonAppointmentApp.Models.User
{
    public class UserInfo : IIdentifiable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string Gender { get; set; }

    }
}
