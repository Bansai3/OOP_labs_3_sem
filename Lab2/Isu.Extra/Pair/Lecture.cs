namespace Isu.Extra;

public class Lecture : Pair
{
    private Flow? _flow;

    public Lecture(TimeOnly startTime, TimeOnly endTime, Lector lector, Auditory auditory, Flow? flow = null)
        : base(startTime, endTime, lector, auditory)
    {
        _flow = flow;
    }
}