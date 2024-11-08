﻿namespace App.Domain.Entities
{
    public class Category : BaseEntity<int>, IAuditEntity
    {
        public string Name { get; set; } = default!;

        public List<Product>? Products { get; set; }
        public DateTime Created { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? Updated { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
