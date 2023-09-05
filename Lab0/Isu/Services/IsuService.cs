using System.Collections.Immutable;
using System.Data;
using CustomException;
using Isu.Entities;
using Isu.Models;
namespace Isu.Services;

public class IsuService : IIsuService
{
    private int _studentsCount = 0;
    private List<Group> _groups;
    private List<Student> _students;

    public IsuService()
    {
        _groups = new List<Group>();
        _students = new List<Student>();
    }

    public bool CheckSimilarGroups(GroupName groupName)
    {
        if (groupName == null) return false;
        return _groups.Any(group => group.GroupName.Name == groupName.Name);
    }

    public Group AddGroup(GroupName name)
    {
        if (CheckSimilarGroups(name))
            throw new SimilarGroupsException($"Group {name.Name} is already in Isu!");
        var newGroup = new Group(name);
        _groups.Add(newGroup);

        return newGroup;
    }

    public Student AddStudent(Group? group, string name)
    {
        if (group == null)
            throw new GroupNotFoundException("There is no such group!");
        var student = new Student(group, name, ++_studentsCount, new CourseNumber(1));
        _students.Add(student);
        return student;
    }

    public void ChangeStudentGroup(Student? student, Group? newGroup)
    {
        if (student == null || newGroup == null)
            throw new NoNullAllowedException("Student and new group are required!");
        student.ChangeGroup(newGroup);
    }

    public Group? FindGroup(GroupName groupName)
    {
        return _groups.SingleOrDefault(gr => gr.GroupName.Name == groupName.Name);
    }

    public ImmutableList<Group> FindGroups(CourseNumber courseNumber)
    {
        return _groups.Where(group => group.CourseNumber.Number == courseNumber.Number).ToImmutableList();
    }

    public Student? FindStudent(int id)
    {
        return _students.SingleOrDefault(student => student.Id == id);
    }

    public ImmutableList<Student> FindStudents(GroupName groupName)
    {
        Group? gr = _groups.SingleOrDefault(group => group.GroupName.Name == groupName.Name);
        return gr == null ? ImmutableList<Student>.Empty : gr.Students;
    }

    public ImmutableList<Student> FindStudents(CourseNumber courseNumber)
    {
        return _students.Where(student => student.CourseNumber.Number == courseNumber.Number).ToImmutableList();
    }

    public Student GetStudent(int id)
    {
       Student? student = _students.SingleOrDefault(student => student.Id == id);
       if (student == null)
           throw new StudentNotFoundException($"There is no student with id: {id}");
       else
           return student;
    }
}