namespace Banks;

public class CreditAccount : Account
{
    public CreditAccount(decimal sum, decimal commissionPercent, int id)
    {
        if (CheckSum(sum) == false)
            throw new ArgumentException("Invalid creditLimit!");
        if (CheckCommission(commissionPercent) == false)
            throw new ArgumentException("Invalid commissionInRubles!");
        if (CheckId(id) == false)
            throw new ArgumentException("Invalid id!");
        Sum = sum;
        CommissionPercent = commissionPercent;
        Id = id;
    }

    public decimal CommissionPercent { get; private set; }

    public decimal CommissionSum { get; private set; }

    public override void TakeOff(decimal sum)
    {
        if (CheckSum(sum) == false)
            throw new ArgumentException("Invalid sum!");
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
        bank.TopUpMoneyOnAccount(accountToTransferIn, sum);
        Sum -= sum;
    }

    public override void AccruePercent()
    {
        if (Sum >= 0)
            return;
        CommissionSum += Math.Abs(CommissionPercent / 100 * Sum);
    }

    public override void AccruePercentSum()
    {
        Sum -= CommissionSum;
        CommissionSum = 0;
    }

    private bool CheckSum(decimal sum) => sum >= 0;

    private bool CheckId(int id) => id > 0;
    private bool CheckCommission(decimal commissionPercent) => commissionPercent > 0;
}