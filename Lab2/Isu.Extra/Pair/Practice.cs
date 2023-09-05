namespace Isu.Extra;

public class Practice : Pair
{
    private IsuExtraGroup? _group;

    public Practice(TimeOnly startTime, TimeOnly endTime, Lector lector, Auditory auditory, IsuExtraGroup? group = null)
        : base(startTime, endTime, lector, auditory)
    {
        _group = group;
    }
}