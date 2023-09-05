using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Data;
using CustomException;
using Isu.Models;
using IsuExtraException;

namespace Isu.Extra;

public class IsuExtraService : IIsuExtraService
{
    private int _studentsCount = 0;
    private List<IsuExtraGroup> _groups;
    private List<IsuExtraStudent> _students;
    private List<MegaFaculty> _megaFaculties;
    private OgnpService _ognpService;

    public IsuExtraService()
    {
        _groups = new List<IsuExtraGroup>();
        _students = new List<IsuExtraStudent>();
        _ognpService = new OgnpService();
        _megaFaculties = new List<MegaFaculty>();
    }

    public ReadOnlyCollection<MegaFaculty> MegaFaculties => new (_megaFaculties);
    public bool CheckSimilarGroups(GroupName groupName)
    {
        if (groupName is null) return false;
        return _groups.Any(group => group.GroupName.Name == groupName.Name);
    }

    public IsuExtraGroup AddGroup(GroupName name, Flow flow)
    {
        if (CheckSimilarGroups(name))
            throw new SimilarGroupsException($"Group {name.Name} is already in Isu!");
        var newGroup = new IsuExtraGroup(name, flow);
        _groups.Add(newGroup);

        return newGroup;
    }

    public MegaFaculty? FindMegaFaculty(MegaFaculty megaFaculty) =>
        _megaFaculties.SingleOrDefault(mg => mg.Title == megaFaculty.Title);

    public MegaFaculty? GetMegaFaculty(MegaFaculty megaFaculty)
    {
        MegaFaculty? searchMegaFaculty = _megaFaculties.SingleOrDefault(mg => mg.Title == megaFaculty.Title);
        if (searchMegaFaculty is null) throw new MegaFacultyTitleException($"Mega faculty {megaFaculty.Title} does not exist!");
        return searchMegaFaculty;
    }

    public IsuExtraStudent AddStudent(IsuExtraGroup? group, string name, MegaFaculty megaFaculty)
    {
        if (group == null)
            throw new GroupNotFoundException("There is no such group!");
        var student = new IsuExtraStudent(group, name, ++_studentsCount, new CourseNumber(1), megaFaculty);
        _students.Add(student);
        return student;
    }

    public void AddMegaFaculty(MegaFaculty megaFaculty)
    {
        if (!CheckMegaFaculty(megaFaculty)) throw new MegaFacultyNullReferenceException("Mega faculty can not be equal to null!");
        if (CheckSimilarMegaFaculty(megaFaculty))
            throw new MegaFacultyTitleException($"Mega faculty {megaFaculty.Title} already exists!");
        _megaFaculties.Add(megaFaculty);
    }

    public void ChangeStudentGroup(IsuExtraStudent? student, IsuExtraGroup? newGroup)
    {
        if (student == null || newGroup == null)
            throw new StudentNullReferenceException("Student and new group are required!");
        student.ChangeGroup(newGroup);
    }

    public IsuExtraGroup? FindGroup(GroupName groupName)
    {
        return _groups.SingleOrDefault(gr => gr.GroupName.Name == groupName.Name);
    }

    public ReadOnlyCollection<IsuExtraGroup> FindGroups(CourseNumber courseNumber)
    {
        return new ReadOnlyCollection<IsuExtraGroup>(_groups.Where(group => group.CourseNumber.Number == courseNumber.Number).ToList());
    }

    public bool ContainsOgnp(Ognp ognp) => _ognpService.ContainsOgnp(ognp);

    public IsuExtraStudent? FindStudent(int id)
    {
        return _students.SingleOrDefault(student => student.Id == id);
    }

    public ReadOnlyCollection<IsuExtraStudent> FindStudents(GroupName groupName)
    {
        IsuExtraGroup? gr = _groups.SingleOrDefault(group => group.GroupName.Name == groupName.Name);
        return gr == null ? new ReadOnlyCollection<IsuExtraStudent>(new List<IsuExtraStudent>(0)) : gr.Students;
    }

    public ReadOnlyCollection<IsuExtraStudent> FindStudents(CourseNumber courseNumber)
    {
        return new ReadOnlyCollection<IsuExtraStudent>(_students.Where(student => student.CourseNumber.Number == courseNumber.Number).ToList());
    }

    public IsuExtraStudent GetStudent(int id)
    {
        IsuExtraStudent? student = _students.SingleOrDefault(student => student.Id == id);
        if (student == null)
           throw new StudentNotFoundException($"There is no student with id: {id}");
        return student;
    }

    public void AddOgnpCourse(Ognp newOgnp, MegaFaculty megaFaculty)
    {
        _ognpService.AddOgnpCourse(newOgnp, megaFaculty);
    }

    public void SubscribeStudentOnOgnp(IsuExtraStudent student, Ognp ognp, Flow flow)
    {
        _ognpService.SubscribeStudentOnOgnp(student, ognp, flow);
    }

    public void UnsubscribeStudentfromOgnp(IsuExtraStudent student, Ognp ognp)
    {
        _ognpService.UnsubscribeStudentfromOgnp(student, ognp);
    }

    public ReadOnlyCollection<Flow> GetFlowsFromOgnpCourse(Ognp ognp)
    {
        return _ognpService.GetFlowsFromOgnpCourse(ognp);
    }

    public ReadOnlyCollection<IsuExtraStudent> GetStudentsFromOgnpGroup(IsuExtraGroup ognpGroup)
    {
        return _ognpService.GetStudentsFromOgnpGroup(ognpGroup);
    }

    public ReadOnlyCollection<IsuExtraStudent> GetUnsubscribedStudentsFromGroup(IsuExtraGroup group)
    {
        return _ognpService.GetUnsubscribedStudentsFromGroup(group);
    }

    private bool CheckMegaFaculty(MegaFaculty? megaFaculty) => megaFaculty is not null;

    private bool CheckSimilarMegaFaculty(MegaFaculty megaFaculty) =>
        _megaFaculties.SingleOrDefault(mf => mf.Title == megaFaculty.Title) is not null;
}