namespace BulkkyBook.Data.Entities
{
    public class Function
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? ParentId { get; set; }
        public int SortOrder { get; set; }
        public string? Icon { get; set; }
        public string? Url { get; set; }
    }
}   