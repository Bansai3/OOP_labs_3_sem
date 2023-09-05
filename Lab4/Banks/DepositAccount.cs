namespace Banks;

public class DepositAccount : Account
{
    public DepositAccount(decimal percent, int id, int durationInDays, decimal sum = 0)
    {
        if (CheckSum(sum) == false)
            throw new ArgumentException("Invalid sum!");
        if (CheckPercent(percent) == false)
            throw new ArgumentException("Invalid percent!");
        if (CheckId(id) == false)
            throw new ArgumentException("Invalid id!");
        if (CheckDuration(durationInDays) == false)
            throw new ArgumentException("Invalid duration!");
        Percent = percent;
        Duration = durationInDays;
        Sum = sum;
        Id = id;
    }

    public decimal Percent { get; private set; }
    public int Duration { get; private set; }
    public decimal PercentSum { get; private set; }

    public override void TakeOff(decimal sum)
    {
        if (Duration != 0)
            return;
        if (CheckSum(sum) == false)
            throw new ArgumentException("Invalid sum!");
        if (Sum - sum < 0)
            throw new ArithmeticException("Current sum is less than the sum to take off");
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
        if (Duration != 0)
            return;
        if (sum > Sum)
            throw new InvalidOperationException("Can not transfer more money than on current account keeps!");
        if (CheckSum(sum) == false)
            throw new ArgumentException("Invalid sum!");
        bank.TopUpMoneyOnAccount(accountToTransferIn, sum);
        Sum -= sum;
    }

    public override void AccruePercent()
    {
        if (Duration == 0)
            return;
        PercentSum += Percent / 100 * Sum;
        Duration--;
    }

    public override void AccruePercentSum()
    {
        if (Duration == 0)
            return;
        Sum += PercentSum;
        PercentSum = 0;
    }

    private bool CheckSum(decimal sum) => sum >= 0;
    private bool CheckPercent(decimal percent) => percent >= 0;

    private bool CheckDuration(int durationInDays) => durationInDays > 0;
    private bool CheckId(int id) => id > 0;
}