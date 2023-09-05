namespace Banks;

public abstract class Account
{
    public int Id { get; protected set; }
    public decimal Sum { get; protected set; }
    public abstract void TakeOff(decimal sum);
    public abstract void TopUp(decimal sum);
    public abstract void Transfer(decimal sum, Bank bank, Account accountToTransferIn);
    public abstract void AccruePercent();
    public abstract void AccruePercentSum();
}