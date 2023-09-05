namespace Banks;

public class BankCharacteristicParameters
{
    public BankCharacteristicParameters(
            decimal debitPercent,
            decimal creditCommissionPercent,
            decimal transferCommission,
            int depositAccountMinDurationInDays,
            int creditLimit,
            int dubiousSum)
    {
        if (CheckPercent(debitPercent) == false)
            throw new ArgumentException("Invalid debit percent!");
        if (CheckPercent(creditCommissionPercent) == false)
            throw new ArgumentException("Invalid commission percent!");
        if (CheckCreditLimit(creditLimit) == false)
            throw new ArgumentException("Invalid credit limit!");
        if (CheckPercent(transferCommission) == false)
            throw new ArgumentException("Invalid transfer commission!");
        if (CheckDubiousSum(dubiousSum) == false)
            throw new ArgumentException("Invalid dubious sum!");
        if (CheckDuration(depositAccountMinDurationInDays) == false)
            throw new ArgumentException("Invalid deposit account minimal duration!");
        DebitPercent = debitPercent;
        CreditCommissionPercent = creditCommissionPercent;
        TransferCommission = transferCommission;
        DepositAccountMinDurationInDays = depositAccountMinDurationInDays;
        CreditLimit = creditLimit;
        DubiousSum = dubiousSum;
    }

    public decimal DebitPercent { get; private set; }
    public decimal CreditCommissionPercent { get; private set; }
    public decimal TransferCommission { get; private set; }
    public int DepositAccountMinDurationInDays { get; private set; }
    public int CreditLimit { get; private set; }
    public int DubiousSum { get; private set; }

    public void SetDebitPercent(decimal newDebitPercent)
    {
        if (CheckPercent(newDebitPercent) == false)
            throw new ArgumentException("Invalid debit percent!");
        DebitPercent = newDebitPercent;
    }

    public void SetCreditCommissionPercent(decimal newCreditCommissionPercent)
    {
        if (CheckPercent(newCreditCommissionPercent) == false)
            throw new ArgumentException("Invalid commission percent!");
        CreditCommissionPercent = newCreditCommissionPercent;
    }

    public void SetTransferCommission(decimal newTransferCommission)
    {
        if (CheckPercent(newTransferCommission) == false)
            throw new ArgumentException("Invalid transfer commission!");
        TransferCommission = newTransferCommission;
    }

    public void SetDepositAccountMinDurationInDays(int newDepositAccountMinDurationInDays)
    {
        if (CheckDuration(newDepositAccountMinDurationInDays) == false)
            throw new ArgumentException("Invalid deposit account minimal duration!");
        DepositAccountMinDurationInDays = newDepositAccountMinDurationInDays;
    }

    public void SetCreditLimit(int newCreditLimit)
    {
        if (CheckCreditLimit(newCreditLimit) == false)
            throw new ArgumentException("Invalid credit limit!");
        CreditLimit = newCreditLimit;
    }

    public void SetDubiousSum(int dubiousSum)
    {
        if (CheckDubiousSum(dubiousSum) == false)
            throw new ArgumentException("Invalid dubious sum!");
        DubiousSum = dubiousSum;
    }

    private bool CheckPercent(decimal percent) => percent > 0;
    private bool CheckCreditLimit(int creditLimit) => creditLimit > 0;

    private bool CheckDubiousSum(int dubiousSum) => dubiousSum > 0;
    private bool CheckDuration(int durationInDays) => durationInDays > 0;
}