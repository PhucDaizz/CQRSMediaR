using Domain.Common;
using Domain.Events;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Product: BaseEntity
    {
        public ProductName Name { get; private set; } = null!;
        public string Description { get; private set; } = string.Empty;
        public Money Price { get; private set; } = null!;
        public int StockQuantity { get; private set; }
        public bool IsActive { get; private set; } = true;
        public Guid CategoryId { get; private set; }

        public Category Category { get; private set; } = null!;

        private Product() { } // For EF Core

        public Product(ProductName name, string description, Money price, int stockQuantity, Guid categoryId)
        {
            Name = name;
            Description = description;
            Price = price;
            StockQuantity = stockQuantity;
            CategoryId = categoryId;

            AddDomainEvent(new ProductCreatedEvent(this));
        }

        public void UpdateDetails(ProductName name, string description, Money price)
        {
            Name = name;
            Description = description;
            Price = price;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new ProductUpdatedEvent(this));
        }

        public void UpdateStock(int quantity)
        {
            if (quantity < 0)
                throw new ArgumentException("Stock quantity cannot be negative");

            StockQuantity = quantity;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
