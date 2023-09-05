using CustomException;
using Isu.Entities;
using Isu.Models;
using Isu.Services;
using Xunit;
namespace Isu.Test;

public class IsuServiceTest
{
    private IsuService _isu = new IsuService();
    private GroupName? _groupName1;
    private GroupName? _groupName2;

    [Fact]
    public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
    {
        var groupName = new GroupName("M3204");
        Group group = _isu.AddGroup(groupName);
        _isu.AddStudent(group, "Volodya");
        Assert.Equal("M3204", _isu.GetStudent(1).Group.GroupName.Name);
        Assert.Equal("Volodya", _isu?.FindGroup(groupName)?.FindStudentById(1)?.Name);
    }

    [Fact]
    public void ReachMaxStudentPerGroup_ThrowException()
    {
        var courseNumber = new CourseNumber(1);
        void AddStudentsToGroup()
        {
            var group = new Group(new GroupName("M3204"));
            for (int i = 1; i <= Group.MaxStudentsAmount + 1; i++)
            {
                _ = new Student(group, $"Fedor{i}", i, courseNumber);
            }
        }

        Assert.Throws<TooManyStudentsInGroupException>(AddStudentsToGroup);
    }

    [Fact]
    public void CreateGroupWithInvalidName_ThrowException()
    {
        void CreateGroup()
        {
            _ = new Group(new GroupName("M320434"));
        }

        Assert.Throws<GroupNameFormatException>(CreateGroup);
    }

    [Fact]
    public void TransferStudentToAnotherGroup_GroupChanged()
    {
         _groupName1 = new GroupName("M3204");
         _groupName2 = new GroupName("M3201");
         var gr1InIsu = _isu.AddGroup(_groupName1);
         var gr2InIsu = _isu.AddGroup(_groupName2);
         var student = _isu.AddStudent(gr1InIsu, "Tom");

         _isu.ChangeStudentGroup(student, gr2InIsu);
         int studentsNumberInM3204 = _isu.FindGroup(_groupName1)?.Students?.Count ?? 0;
         int studentsNumberInM3201 = _isu.FindGroup(_groupName2)?.Students?.Count ?? 0;
         Assert.Equal(0, studentsNumberInM3204);
         Assert.Equal(1, studentsNumberInM3201);
    }

    [Fact]
    public void FindGroupsByCourseNumber()
    {
        var courseNumber = new CourseNumber(1);
        _isu.AddGroup(new GroupName("M3100"));
        _isu.AddGroup(new GroupName("M3101"));
        _isu.AddGroup(new GroupName("M3102"));
        _isu.AddGroup(new GroupName("M3103"));
        _isu.AddGroup(new GroupName("M3104"));
        int expected = 5;
        Assert.Equal(expected, _isu.FindGroups(courseNumber).Count);
    }

    [Fact]
    public void FindStudentsByGroupName()
    {
         _groupName1 = new GroupName("M3100");
         _groupName2 = new GroupName("M3101");
         Group group1 = _isu.AddGroup(_groupName1);
         Group group2 = _isu.AddGroup(_groupName2);
         _isu.AddStudent(group1, "Tom");
         _isu.AddStudent(group2, "Mark");
         Assert.Single(_isu.FindStudents(_groupName1));
         Assert.Single(_isu.FindStudents(_groupName2));
    }

    [Fact]
    public void AddSimilarGroupsToIsu()
    {
        void AddSimilarGroups()
        {
            _isu.AddGroup(new GroupName("M3201"));
            _isu.AddGroup(new GroupName("M3201"));
        }

        Assert.Throws<SimilarGroupsException>(AddSimilarGroups);
    }
}