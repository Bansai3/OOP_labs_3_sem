using System.Collections.ObjectModel;

namespace Banks;

public class Client
{
    public const int AddressParametersCount = 5;
    private List<INotification> _notifications;

    public Client(string name, string surname, string? address, string? passportNumber, int id, bool getCommonNotifications)
    {
        Validate(name, surname, address, passportNumber, id);
        _notifications = new List<INotification>();
        Name = name;
        Surname = surname;
        Address = address;
        PassportNumber = passportNumber;
        GetCommonNotifications = getCommonNotifications;
        Id = id;
    }

    public string? Name { get; private set; }
    public string? Surname { get; private set; }
    public string? Address { get; private set; }
    public string? PassportNumber { get; private set; }
    public bool GetCommonNotifications { get; private set; }
    public int Id { get; private set; }

    public ReadOnlyCollection<INotification> Notifications => new (_notifications);

    public void SetName(string newName)
    {
        if (CheckName(newName) == false)
            throw new ArgumentException("Invalid new name!");
        Name = newName;
    }

    public void SetSurname(string newSurname)
    {
        if (CheckName(newSurname) == false)
            throw new ArgumentException("Invalid new surname!");
        Surname = newSurname;
    }

    public void SetId(int newId)
    {
        if (CheckId(newId) == false)
            throw new ArgumentException("Invalid id!");
        Id = newId;
    }

    public void SetAddress(string? address)
    {
        if (CheckAddress(address) == false)
            throw new ArgumentException("Invalid address!");
        Address = address;
    }

    public void SetPassportNumber(string? passportNumber)
    {
        if (CheckPassportNumber(passportNumber) == false)
            throw new ArgumentException("Invalid passport number!");
        PassportNumber = passportNumber;
    }

    public void Notify(INotification notification)
    {
        if (CheckNotification(notification) == false)
            throw new ArgumentException("Invalid notification type!");
        _notifications.Add(notification);
    }

    public void SetCommonNotification(bool getCommonNotification)
    {
        GetCommonNotifications = getCommonNotification;
    }

    private bool CheckName(string? name) => !string.IsNullOrEmpty(name) && name.All(char.IsLetter);

    private bool CheckAddress(string? address)
    {
        if (address == null)
            return true;
        address = address.Replace(',', ' ');
        string[] data = address.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (data.Length != AddressParametersCount)
            return false;
        if (data[0].All(char.IsLetter) && data[1].All(char.IsLetter) == false)
            return false;
        return data[2].All(char.IsDigit) && data[3].All(char.IsDigit) && data[4].All(char.IsDigit);
    }

    private bool CheckPassportNumber(string? passportNumber) => passportNumber == null || passportNumber.All(char.IsDigit);

    private bool CheckId(int id) => id > 0;

    private bool CheckNotification(INotification? notification) => notification != null;

    private void Validate(string name, string surname, string? address, string? passportNumber, int id)
    {
        if (CheckName(name) == false)
            throw new ArgumentException("Invalid name!");
        if (CheckName(surname) == false)
            throw new ArgumentException("Invalid new surname!");
        if (CheckAddress(address) == false)
            throw new ArgumentException("Invalid address!");
        if (CheckPassportNumber(passportNumber) == false)
            throw new ArgumentException("Invalid passport number!");
        if (CheckId(id) == false)
            throw new ArgumentException("Invalid id!");
    }
}