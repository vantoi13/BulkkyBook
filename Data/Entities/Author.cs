namespace BulkkyBook.Data.Entities
{
    public class Author : BaseEntity
    {
        private string _name = string.Empty;
        private string _biography = string.Empty;
        private DateTime? _birthDate;
        private string _country = string.Empty;
        private string _email = string.Empty;
        private string _photoUrl = string.Empty;
        private bool _isActive = true;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Biography
        {
            get => _biography;
            set => _biography = value;
        }

        public DateTime? BirthDate
        {
            get => _birthDate;
            set => _birthDate = value;
        }

        public string Country
        {
            get => _country;
            set => _country = value;
        }

        public string Email
        {
            get => _email;
            set => _email = value;
        }

        public string PhotoUrl
        {
            get => _photoUrl;
            set => _photoUrl = value;
        }

        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }

        // Navigation property
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}