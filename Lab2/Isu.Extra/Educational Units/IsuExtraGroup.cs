using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using CustomException;
using Isu.Entities;
using Isu.Models;
using IsuExtraException;

namespace Isu.Extra;

public class IsuExtraGroup : Group, IEnumerable<IsuExtraStudent>
{
    private Dictionary<DayOfWeek, List<Practice>> _pairs;
    private List<IsuExtraStudent> _students;

    public IsuExtraGroup(List<IsuExtraStudent> students, GroupName groupName, CourseNumber courseNumber, Flow flow, Dictionary<DayOfWeek, List<Practice>> pairs)
    : base(null, groupName, courseNumber)
    {
        _students = new List<IsuExtraStudent>();
        if (!CheckPractices(pairs)) throw new PairNullReferenceException("Practices can not be null!");
        if (!CheckWeekPracticeIntersection(pairs)) throw new PairIntersectionException("Practices intersect!");
        if (!CheckLectureIntersection(pairs)) throw new PairIntersectionException("Practices intersect with lectures!");
        _pairs = new Dictionary<DayOfWeek, List<Practice>>(pairs);
        if (!CheckFlow(flow)) throw new FlowNullReferenceException("Flow can not be null!");
        flow.AddGroup(this);
        Flow = flow;
        InitStudentsList(_students);
     }

    public IsuExtraGroup(GroupName groupName, Flow flow)
        : base(groupName)
     {
         _pairs = new Dictionary<DayOfWeek, List<Practice>>();
         _students = new List<IsuExtraStudent>();
         if (!CheckFlow(flow)) throw new FlowNullReferenceException("Flow can not be null!");
         flow.AddGroup(this);
         Flow = flow;
     }

    public ReadOnlyDictionary<DayOfWeek, List<Practice>> Pairs => new (_pairs);
    public Flow Flow { get; private set; }
    public new ReadOnlyCollection<IsuExtraStudent> Students => new ReadOnlyCollection<IsuExtraStudent>(_students);

    public void ChangeFlow(Flow newFlow)
    {
        if (!CheckFlow(newFlow)) throw new FlowNullReferenceException("Flow can not be null!");
        newFlow.AddGroup(this);
        Flow.DeleteGroup(this);
        Flow = newFlow;
    }

    public new IEnumerator<IsuExtraStudent> GetEnumerator()
    {
        if (_students.Count == 0)
            yield break;
        for (int i = 0; i < _students.Count; i++)
        {
            yield return _students[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void AddStudent(IsuExtraStudent? student, bool isOgnpStudent = false)
    {
        if (student == null)
            throw new StudentNullReferenceException("Student is required!");
        if (student.Group.GroupName.Name != GroupName.Name && !isOgnpStudent)
            throw new AddStudentWhoIsInAnotherGroupException("Student can not be added to this group because he is in another group now!");
        if (CheckSimilarStudents(student))
            throw new SimilarStudentsException("Such student is already in the group!");
        if (!CheckStudentsAmount())
            throw new TooManyStudentsInGroupException("Too many students in a group!");
        _students.Add(student);
    }

    public new IsuExtraStudent? FindStudentById(int id)
    {
        return _students.SingleOrDefault(student => student.Id == id);
    }

    public void DeleteStudent(IsuExtraStudent student)
    {
        if (student == null) throw new StudentNullReferenceException("Student is required!");
        _students.Remove(student);
    }

    private bool CheckStudentsAmount() => _students.Count < MaxStudentsAmount;

    private bool CheckSimilarStudents(Student student) =>
        _students.Any(st => st.Id == student.Id && st.Name == student.Name);

    private bool CheckFlow(Flow? flow) => flow is not null;

    private bool CheckWeekPracticeIntersection(Dictionary<DayOfWeek, List<Practice>> pairs) =>
        pairs.Any(prList => CheckDayPracticeIntersection(prList.Value));

    private bool CheckPractices(Dictionary<DayOfWeek, List<Practice>>? pairs) => pairs is not null;
    private bool CheckDayPracticeIntersection(List<Practice> practices)
    {
        foreach (Practice practice in practices)
        {
            TimeOnly endTime = practice.EndTime;
            bool practiceIntersection = practices.Any(pr => endTime.IsBetween(pr.StartTime, pr.EndTime));
            if (practiceIntersection) return true;
        }

        return false;
    }

    private bool CheckLectureIntersection(Dictionary<DayOfWeek, List<Practice>> pairs)
    {
        ReadOnlyDictionary<DayOfWeek, List<Lecture>> lectures = Flow.Lectures;
        for (DayOfWeek day = DayOfWeek.Monday; day != DayOfWeek.Sunday; day++)
        {
            if (pairs[day].Any(pair => lectures[day].Any(lec => pair.EndTime.IsBetween(lec.StartTime, lec.EndTime))))
                return false;
        }

        return true;
    }

    private void InitStudentsList(List<IsuExtraStudent>? students)
    {
        if (students == null) return;
        foreach (IsuExtraStudent? student in students)
        {
            AddStudent(student);
        }

        if (!CheckStudentsAmount())
            throw new TooManyStudentsInGroupException("Too many students in a group!");
    }
}