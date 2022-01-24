namespace Gradebook.Models.ViewModels
{
    public class StudentViewModel : Account
    {
        public string Id { get; set; }
        public string ParentId { get; set; }

        public StudentViewModel() { }

        public StudentViewModel(string id, string name, string surname, string email, string phoneNumber, string parentId)
            : base(name, surname, email, phoneNumber)
        {
            Id = id;
            ParentId = parentId;
        }
    }
}