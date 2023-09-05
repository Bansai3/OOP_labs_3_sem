using System.Data;
using CustomException;
using Isu.Models;

namespace Isu.Entities;

public class Student
{
    public const int MinId = 1;
    public Student(Group group, string name, int id, CourseNumber courseNumber)
    {
        Group = group != null ? group : throw new NoNullAllowedException("Group is required!");
        Group.AddStudent(this);
        Name = !string.IsNullOrEmpty(name) ? name : throw new NoNullAllowedException("Invalid name format!");
        Id = id >= MinId ? id : throw new NegativeIdException("Id must be positive!");
        CourseNumber = courseNumber != null ? courseNumber : throw new CourseNumberException();
    }

    public Group Group { get; private set; }
    public string Name { get; private set; }
    public int Id { get; private set; }
    public CourseNumber CourseNumber { get; private set; }

    public void ChangeId(int id)
    {
        Id = id >= Student.MinId ? id : throw new NegativeIdException("Id must be positive!");
    }

    public void ChangeGroup(Group group)
    {
        if (group == null)
            throw new NoNullAllowedException("Group is required!");
        Group pastGroup = Group;
        Group = group;
        Group.AddStudent(this);
        pastGroup.DeleteStudent(this);
    }

    public void ChangeCourseNumber(CourseNumber courseNumber)
    {
        CourseNumber = courseNumber != null ? courseNumber : throw new CourseNumberException();
    }

    public void ChangeName(string name)
    {
        Name = !string.IsNullOrEmpty(name) ? name : throw new NoNullAllowedException("Invalid name format!");
    }
}