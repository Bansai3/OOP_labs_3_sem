using System.Collections.Immutable;
using Isu.Entities;
using Isu.Models;
namespace Isu.Services;
public interface IIsuService
{
    Group AddGroup(GroupName name);
    Student AddStudent(Group group, string name);

    Student GetStudent(int id);
    Student? FindStudent(int id);
    ImmutableList<Student> FindStudents(GroupName groupName);
    ImmutableList<Student> FindStudents(CourseNumber courseNumber);

    Group? FindGroup(GroupName groupName);
    ImmutableList<Group> FindGroups(CourseNumber courseNumber);

    void ChangeStudentGroup(Student student, Group newGroup);
}