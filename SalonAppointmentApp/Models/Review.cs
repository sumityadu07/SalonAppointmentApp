
namespace SalonAppointmentApp.Models
{
    public class Review : Services.IIdentifiable
    {
        public string Id { get; set; }
        public string AppointmentId { get; set; }
        public string Reviewer { get; set; }
        public string Comment { get; set; }
        //Implemented Id from IIdentifiable class
    }
}
