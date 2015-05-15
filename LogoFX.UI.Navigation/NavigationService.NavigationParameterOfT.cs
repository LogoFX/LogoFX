namespace LogoFX.UI.Navigation
{
    internal sealed partial class NavigationService
    {
        private sealed class NavigationParameter<T> : NavigationParameter
        {
            private readonly INavigationService _service;
            //private readonly bool _noTrack;
            private readonly object _argument;

            public NavigationParameter(INavigationService service/*, bool noTrack*/, object argument)
            {
                _service = service;
                //_noTrack = noTrack;
                _argument = argument;
            }

            public override void Navigate()
            {
                //if (_noTrack)
                //{
                //    _service.NavigateNoTrack<T>(_argument);
                //}
                //else
                //{
                //    _service.Navigate<T>(_argument);
                //}

                _service.Navigate<T>(_argument);
            }
        }
    }
}