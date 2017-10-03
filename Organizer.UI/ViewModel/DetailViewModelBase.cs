using Organizer.UI.Event;
using Prism.Commands;
using Prism.Events;
using System.Threading.Tasks;
using System.Windows.Input;
using System;

namespace Organizer.UI.ViewModel
{
    public abstract class DetailViewModelBase : ViewModelBase, IDetailViewModel
    {
        public DetailViewModelBase(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
        }
        public abstract Task LoadAsync(int? id);
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        protected abstract void OnDeleteExecute();
        protected abstract bool OnSaveCanExecute();
        protected abstract void OnSaveExecute();

        protected readonly IEventAggregator EventAggregator;

        private bool _hasChanges;
        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        private int _id;
        public int Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        protected virtual void RaiseDetailDeletedEvent(int modelId)
        {
            EventAggregator.GetEvent<AfterDetailDeletedEvent>().Publish(new AfterDetailDeletedEventArgs
            {
                Id = modelId,
                ViewModelName = this.GetType().Name
            });
        }

        protected virtual void RaiseDetailSavedEvent(int modelId, string displayMember)
        {
            EventAggregator.GetEvent<AfterDetailSavedEvent>().Publish(new AfterDetailSavedEventArgs()
            {
                Id = modelId,
                DisplayMember = displayMember,
                ViewModelName = this.GetType().Name
            });
        }
    }
}
