using System.Collections.ObjectModel;

namespace Banks;

public class CentralBank
{
    private static CentralBank? _centralBank;

    private List<Bank> _banks;

    private CentralBank()
    {
        _banks = new List<Bank>();
    }

    public ReadOnlyCollection<Bank> Banks => new (_banks);

    public static CentralBank GetCentralBankInstance()
    {
        if (_centralBank == null)
            _centralBank = new CentralBank();
        return _centralBank;
    }

    public Bank RegisterBank(
        Dictionary<int, decimal> depositPercents,
        decimal debitPercent,
        decimal creditCommissionPercent,
        int creditLimit,
        decimal transferCommission,
        int dubiousSum,
        int depositAccountMinDurationInDays,
        string title)
    {
        if (CheckTitle(title) == false)
            throw new ArgumentException("Invalid title!");
        if (CheckSimilarTitles(title) == false)
            throw new ArgumentException($"Bank with title {title} already exists!");
        var newBank = new Bank(depositPercents, debitPercent, creditCommissionPercent, creditLimit, transferCommission, dubiousSum, depositAccountMinDurationInDays, title);
        _banks.Add(newBank);
        return newBank;
    }

    public void AccrueCommissionAndPercents(int durationDays)
    {
        var currentDate = new DateTime(2022, 01, 01);
        for (int i = 0; i < durationDays; i++)
        {
            bool endOfMonth = currentDate.Day == DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            foreach (Bank bank in _banks)
            {
                bank.UpdatePercentsAndCommission(endOfMonth);
            }

            currentDate = currentDate.AddDays(1);
        }
    }

    public Bank GetBankById(int id)
    {
        if (CheckId(id) == false)
            throw new ArgumentException("Invalid id!");
        return _banks[id];
    }

    public Bank? FindBank(string title) => _banks.FirstOrDefault(bank => bank.Title == title);

    private bool CheckSimilarTitles(string title) => _banks.All(bank => bank.Title != title);
    private bool CheckTitle(string? title) => title != null;
    private bool CheckId(int id) => id >= 0 && id < _banks.Count;
}