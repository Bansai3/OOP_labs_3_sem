using IsuExtraException;

namespace Isu.Extra;

public class Lector
{
    public Lector(string name, string subject)
    {
        if (!CheckName(name)) throw new LectorNameException("Invalid name format!");
        Name = name;
        if (!CheckSubject(subject)) throw new OgnpTitleException("Invalid ognp title format!");
        Subject = subject;
    }

    public string Name { get; }
    public string Subject { get; }

    private bool CheckName(string name)
    {
        if (string.IsNullOrEmpty(name)) throw new LectorNameException("Invalid name format!");
        string[] fullName = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return fullName.All(str => str.All(char.IsLetter));
    }

    private bool CheckSubject(string title)
    {
        if (string.IsNullOrEmpty(title)) return false;
        string[] fullTitle = title.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return fullTitle.All(str => str.All(char.IsLetter));
    }
}