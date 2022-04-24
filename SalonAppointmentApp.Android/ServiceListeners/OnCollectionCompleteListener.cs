using Android.Gms.Tasks;
using Firebase.Firestore;
using SalonAppointmentApp.Droid.Extensions;
using SalonAppointmentApp.Services;
using System.Collections.ObjectModel;

namespace SalonAppointmentApp.Droid.ServiceListeners
{
    public class OnCollectionCompleteListener<T> : Java.Lang.Object, IOnCompleteListener where T : IIdentifiable
    {
        private System.Threading.Tasks.TaskCompletionSource<ObservableCollection<T>> _tcs;

        public OnCollectionCompleteListener(System.Threading.Tasks.TaskCompletionSource<ObservableCollection<T>> tcs)
        {
            _tcs = tcs;
        }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                var docsObj = task.Result;
                if (docsObj is QuerySnapshot docs)
                {
                    _tcs.TrySetResult(docs.Convert<T>());
                }
            }
        }
    }
}
