using Autofac;
using Organizer.DataAccess;
using Organizer.UI.Data;
using Organizer.UI.ViewModel;

namespace Organizer.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<OrganizerDbContext>().AsSelf();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<PersonDataService>().As<IPersonDataService>();

            return builder.Build();
        }
    }
}
