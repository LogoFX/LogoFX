using LogoFX.Client.Tests.Contracts;
using TestStack.White;

namespace LogoFX.Client.Tests.EndToEnd
{
    public class StartApplicationService : IStartApplicationService
    {
        public void StartApplication(string startupPath)
        {
            BuildersCollectionContext.SerializeBuilders();
            ApplicationContext.Application = Application.Launch(startupPath);
            ApplicationContext.Application.WaitWhileBusy();
        }
    }

    //for now
}
