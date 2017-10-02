using Organizer.Model;

namespace Organizer.UI.Wrapper
{
    public class PersonPhoneNumberWrapper : ModelWrapper<PersonPhoneNumber>
    {
        public PersonPhoneNumberWrapper(PersonPhoneNumber model) : base(model)
        {
        }

        public string Number
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
