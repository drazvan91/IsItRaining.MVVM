using Caliburn.Micro;
using IsItRaining.Pages;

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
