using Autofac;
using Organizer.UI.Data;
using Organizer.UI.ViewModel;

namespace Organizer.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<PersonDataService>().As<IPersonDataService>();

            return builder.Build();
        }
    }
}
