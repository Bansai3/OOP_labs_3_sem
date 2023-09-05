using IsuExtraException;

namespace Isu.Extra;

public class Auditory
{
    public const int MinNumber = 1100;
    public const int MaxNumber = 4499;
    public const int MinFloorNumber = 1;
    public const int MaxFloorNumber = 4;
    public Auditory(int number)
    {
        if (!CheckNumber(number)) throw new AuditoryNumberException("Invalid auditory number!");
        Number = number;
    }

    public int Number { get; private set; }

    public void ChangeNumber(int newNumber)
    {
        if (!CheckNumber(newNumber)) throw new AuditoryNumberException("Invalid auditory number!");
        Number = newNumber;
    }

    private bool CheckNumber(int number)
    {
        if (number is not(>= MinNumber and <= MaxNumber)) return false;
        return GetFloorNumber(number) is >= MinFloorNumber and <= MaxFloorNumber;
    }

    private int GetFloorNumber(int number) => (number / 100) % 10;
}