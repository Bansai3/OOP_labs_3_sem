using System.Collections;
using System.Collections.Immutable;
using System.Data;
using CustomException;
using Isu.Models;

namespace Isu.Entities;

 public class Group : IEnumerable<Student>
 {
     public const int MaxStudentsAmount = 20;
     public static readonly CourseNumber DefaultCourseNumber = new CourseNumber(1);

     public Group(List<Student>? students, GroupName groupName, CourseNumber courseNumber)
     {
         GroupName = groupName != null ? groupName : throw new NoNullAllowedException("Group name is required!");
         CourseNumber = courseNumber != null ? courseNumber : throw new NoNullAllowedException("Course number is required!");
         Students = ImmutableList<Student>.Empty;
         InitStudentsList(students);
     }

     public Group(GroupName groupName)
     {
         GroupName = groupName != null ? groupName : throw new NoNullAllowedException("Group name is required!");
         CourseNumber = DefaultCourseNumber;
         Students = ImmutableList<Student>.Empty;
     }

     public ImmutableList<Student> Students { get; private set; }
     public GroupName GroupName { get; private set; }
     public CourseNumber CourseNumber { get; private set; }

     public void ChangeGroupName(GroupName groupName)
     {
         GroupName = groupName != null ? groupName : throw new NoNullAllowedException("Group name is required!");
     }

     public void ChangeCourseNumber(CourseNumber courseNumber)
     {
         CourseNumber = courseNumber;
     }

     public void AddStudent(Student? student)
     {
         if (student == null)
             throw new NoNullAllowedException("Student is required!");
         if (student.Group.GroupName.Name != GroupName.Name)
             throw new AddStudentWhoIsInAnotherGroupException("Student can not be added to this group because he is in another group now!");
         if (CheckSimilarStudents(student))
             throw new SimilarStudentsException("Such student is already in the group!");
         if (!CheckStudentsAmount())
             throw new TooManyStudentsInGroupException("Too many students in a group!");
         Students = Students.Add(student);
     }

     public IEnumerator<Student> GetEnumerator()
     {
         if (Students.Count == 0)
             yield break;
         for (int i = 0; i < Students.Count; i++)
         {
             yield return Students[i];
         }
     }

     public Student? FindStudentById(int id)
     {
         return Students.SingleOrDefault(student => student.Id == id);
     }

     public void DeleteStudent(Student student)
     {
         if (student == null) throw new NoNullAllowedException("Student is required!");
         Students = Students.Remove(student);
     }

     IEnumerator IEnumerable.GetEnumerator()
     {
         return GetEnumerator();
     }

     private void InitStudentsList(List<Student>? students)
     {
         if (students == null) return;
         foreach (Student? student in students)
         {
             AddStudent(student);
         }

         if (!CheckStudentsAmount())
             throw new TooManyStudentsInGroupException("Too many students in a group!");
     }

     private bool CheckStudentsAmount() => Students.Count < MaxStudentsAmount;

     private bool CheckSimilarStudents(Student student)
     {
         return Students.Any(st => st.Id == student.Id && st.Name == student.Name);
     }
 }
