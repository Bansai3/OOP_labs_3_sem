using IsuExtraException;

namespace Isu.Extra;

public class Pair
{
    private readonly TimeOnly _earliestStartTime = new TimeOnly(8, 20);
    private readonly TimeOnly _earliestEndTime = new TimeOnly(9, 50);
    private readonly TimeOnly _latestStartTime = new TimeOnly(19, 30);
    private readonly TimeOnly _latestEndTime = new TimeOnly(21, 00);
    private readonly TimeSpan _pairDuration = new TimeSpan(1, 30, 0);

    public Pair(TimeOnly startTime, TimeOnly endTime, Lector lector, Auditory auditory)
    {
        if (!CheckTime(startTime, endTime)) throw new PairTimeException("Invalid pair time format!");
        StartTime = startTime;
        EndTime = endTime;
        if (!CheckLector(lector)) throw new LectorNullReferenceException("Lector is required!");
        Lector = lector;
        if (!CheckAuditory(auditory)) throw new AuditoryNullReferenceException("Auditory is required!");
        Auditory = auditory;
    }

    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    public Lector Lector { get; private set; }
    public Auditory Auditory { get; private set; }

    public void ChangeTime(TimeOnly newStartTime, TimeOnly newEndTime)
    {
        if (!CheckTime(newStartTime, newEndTime)) throw new PairTimeException("Invalid pair time format!");
        StartTime = newStartTime;
        EndTime = newEndTime;
    }

    public void ChangeLector(Lector lector)
    {
        if (!CheckLector(lector)) throw new LectorNullReferenceException("Lector is required!");
        Lector = lector;
    }

    public void ChangeAuditory(Auditory auditory)
    {
        if (!CheckAuditory(auditory)) throw new AuditoryNullReferenceException("Auditory is required!");
        Auditory = auditory;
    }

    private bool CheckTime(TimeOnly startTime, TimeOnly endTime)
    {
        if (!(startTime.CompareTo(_earliestStartTime) >= 0 && startTime.CompareTo(_latestStartTime) <= 0))
            return false;
        if (!(endTime.CompareTo(_earliestEndTime) >= 0 && endTime.CompareTo(_latestEndTime) <= 0))
            return false;
        TimeOnly supposedPairEndTime = startTime.Add(_pairDuration);
        return endTime.CompareTo(supposedPairEndTime) == 0;
    }

    private bool CheckLector(Lector? lector) => lector is not null;

    private bool CheckAuditory(Auditory? auditory) => auditory is not null;
}