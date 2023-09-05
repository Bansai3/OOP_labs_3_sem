namespace Banks;

public class ConcreteClientBuilder : ClientBuilder
{
    private Client? _client;
    private string? _name;
    private string? _surname;
    private string? _address;
    private string? _passportNumber;
    private int _id;
    private bool _getCommonNotifications;

    public override ConcreteClientBuilder BuildName(string name)
    {
        if (CheckName(name) == false)
            throw new ArgumentException("Invalid new name!");
        _name = name;
        return this;
    }

    public override ConcreteClientBuilder BuildSurname(string surname)
    {
        if (CheckName(surname) == false)
            throw new ArgumentException("Invalid new surname!");
        _surname = surname;
        return this;
    }

    public override ConcreteClientBuilder BuildAddress(string? address)
    {
        if (CheckAddress(address) == false)
            throw new ArgumentException("Invalid address!");
        _address = address;
        return this;
    }

    public override ConcreteClientBuilder BuildPassportNumber(string? passportNumber)
    {
        if (CheckPassportNumber(passportNumber) == false)
            throw new ArgumentException("Invalid passport number!");
        _passportNumber = passportNumber;
        return this;
    }

    public override ConcreteClientBuilder BuildId(int id)
    {
        if (CheckId(id) == false)
            throw new ArgumentException("Invalid id!");
        _id = id;
        return this;
    }

    public override ConcreteClientBuilder BuildCommonNotifications(bool getCommonNotifications)
    {
        _getCommonNotifications = getCommonNotifications;
        return this;
    }

    public override Client BuildClient()
    {
        if (_name == null)
            throw new ArgumentException("Invalid name!");
        if (_surname == null)
            throw new ArgumentException("Invalid surname!");
        Validate(_name, _surname, _address, _passportNumber, _id);
        return _client = new Client(_name, _surname, _address, _passportNumber, _id, _getCommonNotifications);
    }

    public override bool CheckName(string? name) => !string.IsNullOrEmpty(name) && name.All(char.IsLetter);

    public override bool CheckAddress(string? address)
    {
        if (address == null)
            return true;
        address = address.Replace(',', ' ');
        string[] data = address.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (data.Length != Client.AddressParametersCount)
            return false;
        if (data[0].All(char.IsLetter) && data[1].All(char.IsLetter) == false)
            return false;
        return data[2].All(char.IsDigit) && data[3].All(char.IsDigit) && data[4].All(char.IsDigit);
    }

    public override bool CheckPassportNumber(string? passportNumber) => passportNumber == null || passportNumber.All(char.IsDigit);

    public override bool CheckId(int id) => id > 0;

    public override bool CheckNotification(INotification? notification) => notification != null;

    private void Validate(string? name, string? surname, string? address, string? passportNumber, int id)
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