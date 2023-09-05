namespace Banks;

public class DebitAccount : Account
{
    public DebitAccount(decimal percent,  int id, decimal sum = 0)
    {
        if (CheckSum(sum) == false)
            throw new ArgumentException("Invalid sum!");
        if (CheckPercent(percent) == false)
            throw new ArgumentException("Invalid percent!");
        if (CheckId(id) == false)
            throw new ArgumentException("Invalid id!");
        Sum = sum;
        Percent = percent;
    }

    public decimal Percent { get; private set; }

    public decimal PercentSum { get; private set; }

    public override void TakeOff(decimal sum)
    {
        if (Sum - sum < 0)
            throw new ArithmeticException("Current sum is less than the sum to take off!");
        if (CheckSum(sum) == false)
            throw new ArgumentException("Sum to take off can not be negative");
        Sum -= sum;
    }

    public override void TopUp(decimal sum)
    {
        if (CheckSum(sum) == false)
            throw new ArgumentException("Invalid sum!");
        Sum += sum;
    }

    public override void Transfer(decimal sum, Bank bank, Account accountToTransferIn)
    {
        if (CheckSum(sum) == false)
            throw new ArgumentException("Invalid sum!");
        if (sum > Sum)
            throw new InvalidOperationException("Can not transfer more money than on current account keeps!");
        bank.TopUpMoneyOnAccount(accountToTransferIn, sum);
        Sum -= sum;
    }

    public override void AccruePercent()
    {
        PercentSum += Percent / 100 * Sum;
    }

    public override void AccruePercentSum()
    {
        Sum += PercentSum;
        PercentSum = 0;
    }

    private bool CheckSum(decimal sum) => sum >= 0;
    private bool CheckPercent(decimal percent) => percent >= 0;

    private bool CheckId(int id) => id > 0;
}