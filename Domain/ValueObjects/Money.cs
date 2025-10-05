namespace Domain.ValueObjects
{
    public record Money(decimal Amount, string Currency)
    {
        public static Money Zero(string currency) => new(0, currency);

        public Money Add(Money other)
        {
            if (Currency != other.Currency)
                throw new ArgumentException("Cannot add different currencies");

            return new Money(Amount + other.Amount, Currency);
        }

        public Money Subtract(Money other)
        {
            if (Currency != other.Currency)
                throw new ArgumentException("Cannot subtract different currencies");

            return new Money(Amount - other.Amount, Currency);
        }
    }
}
