
using System.Threading.Tasks;

namespace SalonAppointmentApp.Helpers
{
    public static class AwaitExtension
    {
        public async static void Await(this Task task)
        {
            await task;
        }
    }
}
