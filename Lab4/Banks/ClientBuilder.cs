namespace Banks;

public abstract class ClientBuilder : IClientValidation
{
    public abstract ClientBuilder BuildName(string name);
    public abstract ClientBuilder BuildSurname(string surname);
    public abstract ClientBuilder BuildAddress(string? address);
    public abstract ClientBuilder BuildPassportNumber(string? passportNumber);
    public abstract ClientBuilder BuildId(int id);
    public abstract ClientBuilder BuildCommonNotifications(bool getCommonNotifications);

    public abstract Client BuildClient();

    public abstract bool CheckName(string? name);

    public abstract bool CheckAddress(string? address);

    public abstract bool CheckPassportNumber(string? passportNumber);

    public abstract bool CheckId(int id);

    public abstract bool CheckNotification(INotification? notification);
}