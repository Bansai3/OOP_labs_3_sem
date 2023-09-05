using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Isu.Models;

namespace Isu.Extra;
public interface IIsuExtraService
{
    IsuExtraGroup AddGroup(GroupName name, Flow flow);
    IsuExtraStudent AddStudent(IsuExtraGroup group, string name, MegaFaculty megaFaculty);

    IsuExtraStudent GetStudent(int id);
    IsuExtraStudent? FindStudent(int id);
    ReadOnlyCollection<IsuExtraStudent> FindStudents(GroupName groupName);
    ReadOnlyCollection<IsuExtraStudent> FindStudents(CourseNumber courseNumber);

    IsuExtraGroup? FindGroup(GroupName groupName);
    ReadOnlyCollection<IsuExtraGroup> FindGroups(CourseNumber courseNumber);

    void ChangeStudentGroup(IsuExtraStudent student, IsuExtraGroup newGroup);
}