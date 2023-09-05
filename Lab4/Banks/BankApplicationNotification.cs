namespace Banks;

public class BankApplicationNotification : INotification
{
    private string _message;

    public BankApplicationNotification(string message)
    {
        if (CheckMessage(message) == false)
            throw new ArgumentException("Invalid message type!");
        _message = message;
    }

    public void Notify()
    {
        Console.WriteLine($"Message from bank application/n: {_message}");
    }

    private bool CheckMessage(string message) => string.IsNullOrEmpty(message) == false;
}