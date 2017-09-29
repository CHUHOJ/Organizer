using System.Threading.Tasks;

namespace Organizer.UI.ViewModel
{
    public interface IPersonDetailViewModel
    {
        Task LoadAsync(int personId);
        bool HasChanges { get; }
    }
}