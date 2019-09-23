using Caliburn.Micro;
using RMDesktopUI.EventModels;

namespace RMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private IEventAggregator _eventAggregator;
        private SimpleContainer _container;
        private SalesViewModel _salesViewModel;

        public ShellViewModel(IEventAggregator eventAggregator, SimpleContainer container, SalesViewModel salesVM)
        {
            _container = container;
            _salesViewModel = salesVM;
            ActivateItem(_container.GetInstance<LoginViewModel>());
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(_salesViewModel);
        }
    }
}
