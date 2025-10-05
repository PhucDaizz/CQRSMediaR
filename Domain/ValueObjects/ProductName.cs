namespace Domain.ValueObjects
{
    public record ProductName
    {
        public string Value { get; }

        public ProductName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Product name cannot be empty", nameof(value));

            if (value.Length > 100)
                throw new ArgumentException("Product name cannot exceed 100 characters", nameof(value));

            Value = value.Trim();
        }

        public static implicit operator string(ProductName productName) => productName.Value;
        public static implicit operator ProductName(string value) => new(value);
    }
}
