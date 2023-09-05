namespace Backups.Extra;

public class Requirements
{
    public const int All = 0;
    public const int OneAndMore = 1;
    public Requirements(int requirementsType)
    {
        if (CheckRequirementsType(requirementsType) == false)
            throw new ArgumentException("Invalid requirements type!");
        RequirementsType = requirementsType;
    }

    public int RequirementsType { get; private set; }

    public void ChangeRequirementsType(int newRequirementsType)
    {
        if (CheckRequirementsType(newRequirementsType) == false)
            throw new ArgumentException("Invalid requirements type!");
        RequirementsType = newRequirementsType;
    }

    private bool CheckRequirementsType(int requirementsType) =>
        requirementsType == All || requirementsType == OneAndMore;
}