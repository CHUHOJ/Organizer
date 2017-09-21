using Autofac;
using Organizer.DataAccess;
using Organizer.UI.Data;
using Organizer.UI.ViewModel;
using Prism.Events;

namespace Organizer.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<OrganizerDbContext>().AsSelf();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<PersonDetailViewModel>().As<IPersonDetailViewModel>();

            builder.RegisterType<PersonLookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<PersonDataService>().As<IPersonDataService>();

            return builder.Build();
        }
    }
}
