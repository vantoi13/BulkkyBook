namespace BulkkyBook.Data.Entities
{
    public class Category : BaseEntity
    {
        private string _name = string.Empty;
        private string _description = string.Empty;
        private int _displayOrder = 1;
        private string _iconClass = "fas fa-tag";
        private bool _isActive = true;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Description
        {
            get => _description;
            set => _description = value;
        }

        public int DisplayOrder
        {
            get => _displayOrder;
            set => _displayOrder = value;
        }

        public string IconClass
        {
            get => _iconClass;
            set => _iconClass = value;
        }

        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }

        // Navigation properties
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}