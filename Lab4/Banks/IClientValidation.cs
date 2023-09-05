namespace Banks;

public interface IClientValidation
{
     bool CheckName(string? name);

     bool CheckAddress(string? address);
     bool CheckPassportNumber(string? passportNumber);

     bool CheckId(int id);

     bool CheckNotification(INotification? notification);
}