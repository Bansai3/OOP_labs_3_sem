namespace Isu.Models;
using CustomException;
public class CourseNumber
{
    public const int MinCourseNumber = 1;
    public const int MaxCourseNumber = 4;
    public CourseNumber(int number)
     {
         Number = CheckNumber(number) ? number : throw new CourseNumberException("Invalid course number!");
     }

    public int Number { get; private set; }

    public void ChangeNumber(int number)
    {
        Number = CheckNumber(number) ? number : throw new CourseNumberException("Invalid course number!");
    }

    private bool CheckNumber(int number) => number is >= MinCourseNumber and <= MaxCourseNumber;
}