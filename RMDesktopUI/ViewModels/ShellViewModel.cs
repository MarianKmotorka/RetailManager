using Caliburn.Micro;
using RMDesktopUI.EventModels;

namespace RMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>, IHandle<LoadingEvent>
    {
        private IEventAggregator _eventAggregator;
        private SalesViewModel _salesViewModel;
        private bool _isLoading = false;

        public ShellViewModel(IEventAggregator eventAggregator, SalesViewModel salesVM)
        {
            _salesViewModel = salesVM;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                NotifyOfPropertyChange(() => IsLoading);
            }
        }

        public void Handle(LogOnEvent message)
        {
            IsLoading = false;
            ActivateItem(_salesViewModel);
        }

        public void Handle(LoadingEvent message)
        {
            IsLoading = message.IsLoading;
        }
    }
}
