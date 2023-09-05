using System.Collections.ObjectModel;
using CustomException;
using IsuExtraException;

namespace Isu.Extra;

public class Flow
{
    public const int MinFlowNumber = 1;
    public const int MinNumberOfPlaces = 1;
    public const int MaxNumberOfPlaces = 300;
    private List<IsuExtraGroup> _groups;
    private Dictionary<DayOfWeek, List<Lecture>> _lectures;

    public Flow(int number, int numberOfPlaces, Dictionary<DayOfWeek, List<Lecture>> lectures)
    {
        if (!CheckNumber(number)) throw new FlowNumberException("Invalid flow number format!");
        Number = number;
        if (!CheckNumberOfPlaces(numberOfPlaces)) throw new NumberOfPlacesException("Invalid number of places!");
        NumberOfPlaces = numberOfPlaces;
        _groups = new List<IsuExtraGroup>();
        if (!CheckLectures(lectures)) throw new PairNullReferenceException("Lectures can not be null!");
        if (CheckWeekLectureIntersection(lectures)) throw new PairIntersectionException("Lectures intersect!");
        _lectures = new Dictionary<DayOfWeek, List<Lecture>>(lectures);
    }

    public int Number { get; private set; }
    public int NumberOfPlaces { get; private set; }

    public ReadOnlyCollection<IsuExtraGroup> Groups => new (_groups);
    public ReadOnlyDictionary<DayOfWeek, List<Lecture>> Lectures => new (_lectures);

    public void ChangeNumber(int newNumber)
    {
        if (!CheckNumber(newNumber)) throw new FlowNumberException("Invalid flow number format!");
        Number = newNumber;
    }

    public void ChangeNumberOfPlaces(int newNumberOfPlaces)
    {
        if (!CheckNumberOfPlaces(newNumberOfPlaces)) throw new NumberOfPlacesException("Invalid number of places!");
        NumberOfPlaces = newNumberOfPlaces;
    }

    public void AddGroup(IsuExtraGroup group)
    {
        if (!CheckGroup(group)) throw new GroupNullReferenceException("Group can not be null!");
        if (CheckSimilarGroups(group)) throw new SimilarGroupsException($"Group {group.GroupName.Name} already exist!");
        _groups.Add(group);
    }

    public void DeleteGroup(IsuExtraGroup group)
    {
        if (!CheckGroup(group)) throw new GroupNullReferenceException("Group can not be null!");
        IsuExtraGroup? gr = FindGroup(group.GroupName.Name);
        if (gr is null) throw new GroupNullReferenceException($"There is no group {group.GroupName.Name} in flow!");
        _groups.Remove(group);
    }

    public IsuExtraGroup GetGroup(string groupName)
    {
        IsuExtraGroup? searchGroup = _groups.SingleOrDefault(gr => gr.GroupName.Name == groupName);
        if (searchGroup is null) throw new GroupNullReferenceException($"Group {groupName} does not exist!");
        return searchGroup;
    }

    public IsuExtraGroup? FindGroup(string groupName) => _groups.SingleOrDefault(gr => gr.GroupName.Name == groupName);
    private bool CheckNumber(int number) => number >= MinFlowNumber;

    private bool CheckNumberOfPlaces(int numberOfPlaces) =>
        numberOfPlaces is >= MinNumberOfPlaces and <= MaxNumberOfPlaces;

    private bool CheckWeekLectureIntersection(Dictionary<DayOfWeek, List<Lecture>> lectures) =>
        lectures.Any(lecList => CheckDayLectureIntersection(lecList.Value));

    private bool CheckDayLectureIntersection(List<Lecture> lectures)
    {
        foreach (Lecture lecture in lectures)
        {
            TimeOnly endTime = lecture.EndTime;
            bool lectureIntersection = lectures.Any(lec => endTime.IsBetween(lec.StartTime, lec.EndTime));
            if (lectureIntersection) return true;
        }

        return false;
    }

    private bool CheckGroup(IsuExtraGroup? group) => group is not null;

    private bool CheckSimilarGroups(IsuExtraGroup group) => FindGroup(group.GroupName.Name) is not null;

    private bool CheckLectures(Dictionary<DayOfWeek, List<Lecture>>? lectures) => lectures is not null;
}