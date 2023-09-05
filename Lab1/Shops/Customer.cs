using ShopExceptions;
namespace Shops;

public class Сustomer
{
    public const int MinNameLength = 3;
    public const int MaxNameLength = 50;
    public const double MinMoney = 0;
    public const int NameParametersCount = 3;

    public Сustomer(string name, double money)
    {
        if (!CheckName(name)) throw new CustomerNameFormatException("Incorrect new name format!");
        Name = name;
        if (!CheckMoney(money)) throw new NegativeMoneyException("Negative money!");
        Money = money;
    }

    public string Name { get; private set; }
    public double Money { get; private set; }

    public void AddMoney(double money)
    {
        if (!CheckMoney(money)) throw new NegativeMoneyException("Negative money!");
        Money += money;
    }

    public void SubtractMoney(double money)
    {
        if (!CheckMoney(money)) throw new NegativeMoneyException("Negative money!");
        if (!CheckMoney(Money - money)) throw new NegativeMoneyException($"Person {Name} will have negative amount of money after subtraction!");
        Money -= money;
    }

    public void ChangeName(string newName)
    {
        if (!CheckName(newName)) throw new CustomerNameFormatException("Incorrect new name format!");
        Name = newName;
    }

    private bool CheckName(string? name)
    {
        if (string.IsNullOrEmpty(name)) return false;
        string[] fullName = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (fullName.Length is not NameParametersCount) return false;
        return fullName.All(str =>
            str.All(char.IsLetter) && str.Length is >= MinNameLength and <= MaxNameLength);
    }

    private bool CheckMoney(double money) => money >= MinMoney;
}