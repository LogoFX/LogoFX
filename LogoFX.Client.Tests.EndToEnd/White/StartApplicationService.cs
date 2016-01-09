using LogoFX.Client.Tests.Contracts;
using LogoFX.Client.Tests.EndToEnd.Shared;
using TestStack.White;

namespace LogoFX.Client.Tests.EndToEnd.White
{
    //for now
    public class StartApplicationService : IStartApplicationService
    {
        public void StartApplication(string startupPath)
        {
            BuildersCollectionContext.SerializeBuilders();
            ApplicationContext.Application = Application.Launch(startupPath);
            ApplicationContext.Application.WaitWhileBusy();
        }
    }    
}
