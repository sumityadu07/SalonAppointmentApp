using Android.Gms.Extensions;
using Firebase;
using Firebase.Firestore;
using Java.Util;
using SalonAppointmentApp.Droid;
using SalonAppointmentApp.Droid.ServiceListeners;
using SalonAppointmentApp.Models;
using SalonAppointmentApp.Models.Marketing;
using SalonAppointmentApp.Models.Salon;
using SalonAppointmentApp.Models.User;
using SalonAppointmentApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(Repository<Stylist>))]
[assembly: Dependency(typeof(Repository<Ad>))]
[assembly: Dependency(typeof(Repository<Category>))]
[assembly: Dependency(typeof(Repository<Slot>))]
[assembly: Dependency(typeof(Repository<UserInfo>))]
[assembly: Dependency(typeof(Repository<Address>))]
[assembly: Dependency(typeof(Repository<Post>))]
[assembly: Dependency(typeof(Repository<Appointment>))]
[assembly: Dependency(typeof(Repository<Review>))]
[assembly: Dependency(typeof(Repository<Service>))]

namespace SalonAppointmentApp.Droid
{
    public class Repository<T> : IRepository<T> where T : IIdentifiable
    {
        IAuthentication authService = DependencyService.Resolve<IAuthentication>();
        public string CurrentUid { get; set; }
        public string Phone { get; set; }
        FirebaseFirestore DataStore;
        DocumentReference docRef;
        CollectionReference StylistsCollRef;

        public Repository()
        {
            DataStore = FirebaseFirestore.Instance;
            StylistsCollRef = DataStore.Collection("stylists");
            if (authService.IsSignIn())
            {
                CurrentUid = authService.CurrentUid();
                Phone = authService.Phone();
                docRef = DataStore.Collection("users").Document(CurrentUid);
            }
        }

        #region User
        public async Task Save(string name, string email, string imageUrl, string gender)
        {
            var phone = authService.Phone();
            var userData = new HashMap();
            userData.Put("phone", phone);
            userData.Put("name", name);
            userData.Put("email", email);
            userData.Put("gender", gender);
            userData.Put("image", imageUrl);
            await docRef.Set(userData);
        }

        public async Task Update(string name, string email, string imageUrl, string gender)
        {
            var dictionay = new Dictionary<string, Java.Lang.Object>()
            {
                { "name", name },
                { "gender", gender },
                { "email", email },
                { "image", imageUrl}
            };
            await docRef.Update(dictionay);
        }

        public async Task SaveAddress(string address)
        {
            var docref = DataStore.Collection("addresses").Document();
            var userData = new HashMap();
            userData.Put("userId", CurrentUid);
            userData.Put("location", address);
            await docref.Set(userData);
        }
        public void AddMail(string email)
        {
            throw new NotImplementedException();
        }
        public Task<bool> Delete(string collection, string item)
        {
            var tcs = new TaskCompletionSource<bool>();
            DataStore.Collection(collection).Document(item).Delete().AddOnCompleteListener(new OnDeleteCompleteListener(tcs));
            return tcs.Task;
        }

        public Task<UserInfo> GetUserAsync()
        {
            var tcs = new TaskCompletionSource<UserInfo>();
            docRef.Get().AddOnCompleteListener(new OnCompleteListener(tcs));
            return tcs.Task;
        }
        public Task<ObservableCollection<T>> GetCollectionbyUserId(string collection)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<T>>();
            DataStore.Collection(collection).WhereEqualTo("userId", CurrentUid).Get()
                    .AddOnCompleteListener(new OnCollectionCompleteListener<T>(tcs));
            return tcs.Task;
        }
        #endregion

        #region BasicDataCall for ItemsInCart,Items,Services
        public Task<ObservableCollection<T>> GetCollection(string collection)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<T>>();
            DataStore.Collection(collection).Get()
                    .AddOnCompleteListener(new OnCollectionCompleteListener<T>(tcs));
            return tcs.Task;
        }

        public Task<ObservableCollection<T>> SortItemsbyCategory(string category)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<T>>();

            var query = DataStore.Collection("services");
            query.WhereEqualTo("category", category).OrderBy("name").Limit(10).Get()
                 .AddOnCompleteListener(new OnCollectionCompleteListener<T>(tcs));
            return tcs.Task;
        }
        public Task<ObservableCollection<T>> GetCollectionbyItemUid(string collection, string Uid, string last_doc_id)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<T>>();
            DataStore.Collection(collection)
                .WhereEqualTo("uid", Uid)
                .OrderBy("id")
                .StartAfter(last_doc_id)
                .Limit(10)
                .Get().AddOnCompleteListener(new OnCollectionCompleteListener<T>(tcs));
            return tcs.Task;
        }

        public async Task<T> GetDocument(string collection, string id)
        {
            var tcs = new TaskCompletionSource<T>();
            await DataStore.Collection(collection)
                        .Document(id)
                        .Get().AddOnCompleteListener(new OnDocumentCompleteListener<T>(tcs));
            return await tcs.Task;
        }

        public Task<T> GetDocumentByName(string name)
        {
            var tcs = new TaskCompletionSource<T>();
            DataStore.Collection("services")
                        .WhereEqualTo("name", name)
                        .Get().AddOnCompleteListener(new OnDocumentCompleteListener<T>(tcs));
            return tcs.Task;
        }
        #endregion

        public Task<ObservableCollection<T>> GetStylists(string id, string area = null)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<T>>();
            StylistsCollRef.Get()
                    .AddOnCompleteListener(new OnCollectionCompleteListener<T>(tcs));
            return tcs.Task;
        }

        public async Task UpdateQuantity(string collection, string productId, int quantity)
        {
            var doc = DataStore.Collection(collection).Document(productId);
            await doc.Update("quantity", quantity);
        }
        public async Task SavePost(Post post)
        {
            var doc = DataStore.Collection("savedposts")
                            .Document();
            var map = new HashMap();
            map.Put("imgurl", post.Imgurl);
            map.Put("userId", post.Title);
            map.Put("reviewer", post.Description);
            await doc.Set(map);
        }
        public async Task CancelAppointment(Appointment order)
        {
            var appdoc = DataStore.Collection("appointments").Document(order.OrderId.ToString());
            await appdoc.Update("status", "Cancelled");
        }
        public async Task SaveReview(Review review)
        {
            var doc = DataStore.Collection("reviews")
                               .Document();
            var userData = new HashMap();
            userData.Put("appointmentId", review.AppointmentId);
            userData.Put("reviewer", review.Reviewer);
            userData.Put("comment", review.Comment);

            await doc.Set(userData);
        }

        public async Task SaveAppointment(Appointment appointment)
        {
            var doc = DataStore.Collection("appointments").Document();
            var userData = new HashMap();
            userData.Put("affilatedsalonId", "");
            userData.Put("userId", CurrentUid);
            userData.Put("appointmentId", appointment.OrderId);
            userData.Put("services", appointment.OrderName);
            userData.Put("slot", appointment.OrderDate);
            userData.Put("charge", appointment.OrderTotal);
            userData.Put("status", "Scheduled");
            userData.Put("bookingdate", Timestamp.Now());
            userData.Put("staffId", "");
            userData.Put("staffName", "");
            userData.Put("IsCompleted", false);
            await doc.Set(userData);
        }
    }
}
