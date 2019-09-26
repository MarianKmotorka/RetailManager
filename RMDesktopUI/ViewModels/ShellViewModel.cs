using Caliburn.Micro;
using RMDesktopUI.EventModels;

namespace RMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private IEventAggregator _eventAggregator;
        private SalesViewModel _salesViewModel;

        public ShellViewModel(IEventAggregator eventAggregator, SalesViewModel salesVM)
        {
            _salesViewModel = salesVM;
            _eventAggregator = eventAggregator;

            _eventAggregator.Subscribe(this);

            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(_salesViewModel);
        }
    }
}
