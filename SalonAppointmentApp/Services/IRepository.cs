using SalonAppointmentApp.Models;
using SalonAppointmentApp.Models.Salon;
using SalonAppointmentApp.Models.User;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SalonAppointmentApp.Services
{
    public interface IIdentifiable
    {
        string Id { get; set; }
    }

    public interface IRepository<T> where T : IIdentifiable
    {
        #region User
        string CurrentUid { get; set; }
        string Phone { get; set; }
        Task<ObservableCollection<T>> GetCollectionbyUserId(string collection);
        Task<UserInfo> GetUserAsync();
        Task Save(string name, string email, string imageUrl, string gender);
        Task SaveAddress(string address);
        Task Update(string name, string email, string imageUrl, string gender);
        void AddMail(string email);
        Task<bool> Delete(string collection, string item);
        #endregion
        Task CancelAppointment(Appointment order);
        Task SaveAppointment(Appointment order);
        Task SavePost(Post post);

        Task<ObservableCollection<T>> GetCollection(string collection);
        Task<ObservableCollection<T>> SortItemsbyCategory(string category);
        Task<ObservableCollection<T>> GetCollectionbyItemUid(string collection, string Uid, string last_doc_id = null);
        Task<ObservableCollection<T>> GetStylists(string id, string area = null);
        Task<T> GetDocument(string collection, string id);

        Task<T> GetDocumentByName(string name);

        Task UpdateQuantity(string collection, string productId, int quantity);
        Task SaveReview(Review review);
    }
}
