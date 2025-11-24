namespace ATMOperations.Entities;

internal class Operation
{
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public OperationType OperationType { get; set; }
    public decimal Amount { get; set; }
    public string PersonalNumber { get; set; } = string.Empty;
}

internal enum OperationType
{
    BalanceInquiry,
    Deposit,
    Withdraw
}
