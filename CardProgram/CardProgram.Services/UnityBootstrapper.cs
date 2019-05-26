using CardProgram.Services.Interfaces;
using Unity;
using Unity.Lifetime;

namespace CardProgram.Services
{
    public class UnityBootstrapper
    {
        public static IUnityContainer Initialize()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<ICardServiceProvider, CardServiceProvider>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICardFactory<VirtualCashCard>, CardFactory<VirtualCashCard>>();

            return container;
        }
    }
}