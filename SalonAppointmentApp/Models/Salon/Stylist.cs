namespace SalonAppointmentApp.Models.Salon
{
    public class Stylist : Services.IIdentifiable
    {
        public string Id { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Img { get; set; }
        public string Area { get; set; }
        public string AffilationSalonId { get; set; }
        public string AffilationSalonName { get; set; }

    }
}
