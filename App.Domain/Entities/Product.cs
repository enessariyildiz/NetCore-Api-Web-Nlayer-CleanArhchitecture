using App.Domain.Entities.Common;

namespace App.Domain.Entities
{
    public class Product : BaseEntity<int>, IAuditEntity
    {
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public Category Category { get; set; } = default;
        public DateTime Created { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? Updated { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
