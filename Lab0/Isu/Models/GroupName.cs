using CustomException;
namespace Isu.Models;
public class GroupName
{
    private const int GroupNameLength = 5;
    public GroupName(string name)
    {
        Name = CheckGroupNameFormat(name) ? name : throw new GroupNameFormatException("Invalid group name format!");
    }

    public string Name { get; private set; }

    public void ChangeName(string groupName)
    {
        Name = CheckGroupNameFormat(groupName) ? groupName : throw new GroupNameFormatException("Invalid group name format!");
    }

    private bool CheckGroupNameFormat(string? groupName)
    {
        if (groupName == null || groupName.Length != GroupNameLength || !char.IsUpper(groupName[0]))
            return false;

        for (int i = 1; i < groupName.Length; i++)
        {
            if (!char.IsDigit(groupName[i]))
                return false;
        }

        return true;
    }
}