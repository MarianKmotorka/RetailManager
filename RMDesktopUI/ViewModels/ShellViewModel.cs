using Caliburn.Micro;
using RM.WPF.Library.Models;
using RMDesktopUI.EventModels;

namespace RMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>, IHandle<LoadingEvent>
    {
        private IEventAggregator _eventAggregator;
        private SalesViewModel _salesViewModel;
        private bool _isLoading = false;
        private ILoggedInUserModel _loggedInUser;

        public ShellViewModel(IEventAggregator eventAggregator, SalesViewModel salesVM, ILoggedInUserModel user)
        {
            _salesViewModel = salesVM;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            _loggedInUser = user;
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
        public bool IsLoggedIn => !string.IsNullOrWhiteSpace(_loggedInUser?.Token);

        public void ExitApplication()
        {
            TryClose();
        }

        public void LogOut()
        {
            _loggedInUser.LogOff();
            ActivateItem(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public void Handle(LogOnEvent message)
        {
            IsLoading = false;
            ActivateItem(_salesViewModel);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public void Handle(LoadingEvent message)
        {
            IsLoading = message.IsLoading;
        }
    }
}
