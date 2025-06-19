namespace BulkkyBook.Data.Entities
{
    public interface IBaseEntity
    {
        string Id { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime? UpdatedDate { get; set; }
    }
}