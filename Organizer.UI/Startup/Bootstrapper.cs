using Autofac;
using Organizer.DataAccess;
using Organizer.UI.Data;
using Organizer.UI.Data.Lookups;
using Organizer.UI.Data.Repositories;
using Organizer.UI.View.Services;
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
            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<PersonDetailViewModel>().As<IPersonDetailViewModel>();

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<PersonRepository>().As<IPersonRepository>();

            return builder.Build();
        }
    }
}
