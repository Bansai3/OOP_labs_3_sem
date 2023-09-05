using Xunit;

namespace Banks.Test;

public class BankTest
{
    private Dictionary<int, decimal> _depositPercents = new ()
    {
        [50000] = 3,
        [100000] = 4,
        [150000] = 5,
    };

    private decimal debitPercent = 1;
    private decimal creditCommissionPercent = 1;
    private int creditLimit = 100000;
    private decimal transferCommission = 1;
    private int dubiousSum = 50000;
    private int depositAccountMinDurationInDays = 10;
    private CentralBank _centralBank = CentralBank.GetCentralBankInstance();

    [Fact]
    public void AccruePercentsOnDebitAccount()
    {
        Bank sberBank = _centralBank.RegisterBank(_depositPercents, debitPercent, creditCommissionPercent, creditLimit, transferCommission, dubiousSum, depositAccountMinDurationInDays, "Sberbank");
        Client client = sberBank.AddClient("Tom", "Raddle", "SPB, Sovietskaya, 8, 10, 3");
        DebitAccount account = sberBank.OpenDebitAccount(client, 10000);
        _centralBank.AccrueCommissionAndPercents(32);
        decimal sum = 13100;
        Assert.Equal(sum, account.Sum);
    }

    [Fact]
    public void TryToTakeoffMoneyFromDepositAccountBeforeTheTermEnds()
    {
        Bank alphaBank = _centralBank.RegisterBank(_depositPercents, debitPercent, creditCommissionPercent, creditLimit, transferCommission, dubiousSum, depositAccountMinDurationInDays, "AlphaBank");
        Client client = alphaBank.AddClient("Tom", "Raddle", "SPB, Sovietskaya, 8, 10, 3");
        DepositAccount account = alphaBank.OpenDepositAccount(client, 10, 10000);
        alphaBank.TakeOffMoneyFromAccount(account, 5000);
        Assert.Equal(10000, account.Sum);
    }

    [Fact]
    public void NotifyClient()
    {
        Bank sberBank = _centralBank.RegisterBank(_depositPercents, debitPercent, creditCommissionPercent, creditLimit, transferCommission, dubiousSum, depositAccountMinDurationInDays, "Sber");
        Client client1 = sberBank.AddClient("Tom", "Raddle", "SPB, Sovietskaya, 8, 10, 3");
        Client client2 = sberBank.AddClient("Ben", "Simmons", "LA, GreenStreet, 5, 2, 3");
        DebitAccount account1 = sberBank.OpenDebitAccount(client1, 10000, getNotification: true);
        DebitAccount account2 = sberBank.OpenDebitAccount(client2, 10000, getNotification: true);
        sberBank.ChangeDebitPercent(2);
        Assert.Single(client1.Notifications);
        Assert.Single(client2.Notifications);
    }

    [Fact]
    public void GetCreditCommission()
    {
        Bank sberBank = _centralBank.RegisterBank(_depositPercents, debitPercent, creditCommissionPercent, creditLimit, transferCommission, dubiousSum, depositAccountMinDurationInDays, "SberB");
        Client client2 = sberBank.AddClient("Ben", "Simmons", "LA, GreenStreet, 1, 5, 2", "123456");
        CreditAccount creditAccount = sberBank.OpenCreditAccount(client2, creditLimit);
        sberBank.TakeOffMoneyFromAccount(creditAccount, 110000);
        _centralBank.AccrueCommissionAndPercents(32);
        int sum = -13100;
        Assert.Equal(sum, creditAccount.Sum);
    }
}