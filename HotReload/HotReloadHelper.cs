using System.Reactive.Linq;

namespace CSharpMarkupPeopleInSpaceMaui.HotReload
{
    public class HotReloadHelper : IDisposable
    {
        private readonly Page _page;
        private readonly Action _build;
        private readonly IDisposable _navigatedToSubscription;
        private readonly IDisposable _navigatedFromSubscription;

        public HotReloadHelper(Page page, Action build)
        {
            _page = page;
            _build = build;

            var navigatedTo = Observable.FromEventPattern<NavigatedToEventArgs>(
                handler => _page.NavigatedTo += handler,
                handler => _page.NavigatedTo -= handler
            );

            var navigatedFrom = Observable.FromEventPattern<NavigatedFromEventArgs>(
                handler => _page.NavigatedFrom += handler,
                handler => _page.NavigatedFrom -= handler
            );

            _navigatedToSubscription = navigatedTo.Subscribe(_ => OnNavigatedTo());
            _navigatedFromSubscription = navigatedFrom.Subscribe(_ => OnNavigatedFrom());
        }

        private void OnNavigatedTo()
        {
            _build();

#if DEBUG
            HotReloadService.UpdateApplicationEvent += ReloadUI;
#endif
        }

        private void OnNavigatedFrom()
        {
#if DEBUG
            HotReloadService.UpdateApplicationEvent -= ReloadUI;
#endif
        }

        private void ReloadUI(Type[]? obj)
        {
            _page.Dispatcher.Dispatch(() =>
            {
                _build();
            });
        }

        public void Dispose()
        {
            _navigatedToSubscription.Dispose();
            _navigatedFromSubscription.Dispose();
        }
    }
}