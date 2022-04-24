using SalonAppointmentApp.Models.Salon;
using SalonAppointmentApp.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace SalonAppointmentApp.Pages
{
    public class ExplorePageModel : ViewModelBase
    {
        IRepository<Post> repository = DependencyService.Resolve<IRepository<Post>>();

        public ExplorePageModel()
        {
            posts = new ObservableCollection<Post>();
            LoadPosts();
        }

        public ICommand SaveCommand => new AsyncCommand<Post>(SavePost);
        async void LoadPosts()
        {
            Posts = await repository.GetCollection("posts");
        }

        async Task SavePost(Post post) => await repository.SavePost(post);

        private ObservableCollection<Post> posts;
        public ObservableCollection<Post> Posts
        {
            get { return posts; }
            set => SetProperty(ref posts, value);
        }
    }
}
