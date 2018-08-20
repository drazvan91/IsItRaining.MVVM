using Caliburn.Micro;
using IsItRaining.Pages;
using IsItRaining.Services;

namespace IsItRaining
{
    public class ShellViewModel : Conductor<Screen>
    {
        public ShellViewModel()
        {
            var homePageVm = IoC.Get<HomePageViewModel>();

            ActivateItem(homePageVm);
        }
    }
}
