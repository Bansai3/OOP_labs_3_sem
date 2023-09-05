namespace Banks.Console;

public class BankConsole
{
    private Commands _commands;

    public BankConsole()
    {
        _commands = new Commands();
    }

    public void LaunchConsole()
    {
        System.Console.WriteLine("Bank Interface :)");
        System.Console.WriteLine("Use command help to look at ALL available commands");
        while (true)
        {
            System.Console.Write(">");
            string? command = System.Console.ReadLine();
            _commands.ExecuteCommand(command);
        }
    }
}
