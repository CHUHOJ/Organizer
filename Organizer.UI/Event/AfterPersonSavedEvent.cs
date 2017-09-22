using Prism.Events;

namespace Organizer.UI.Event
{
    public class AfterPersonSavedEvent : PubSubEvent<AfterPersonSavedEventArgs>
    {


    }
    public class AfterPersonSavedEventArgs
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
    }
}
