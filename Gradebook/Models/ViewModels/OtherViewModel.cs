namespace Gradebook.Models.ViewModels
{
    public class OtherViewModel : Account
    {
        public string Id { get; set; }

        public OtherViewModel() { }

        public OtherViewModel(string id, string name, string surname, string email, string phoneNumber)
            : base(name, surname, email, phoneNumber)
        {
            Id = id;
        }
    }
}