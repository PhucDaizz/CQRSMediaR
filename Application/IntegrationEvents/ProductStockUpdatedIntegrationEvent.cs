namespace Application.IntegrationEvents
{
    public record ProductStockUpdatedIntegrationEvent
    {
        public Guid ProductId { get; init; }
        public string ProductName { get; init; } = string.Empty;
        public int OldQuantity { get; init; }
        public int NewQuantity { get; init; }
        public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
    }
}
