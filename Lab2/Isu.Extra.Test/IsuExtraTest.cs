using System.Collections.ObjectModel;
using Isu.Models;
using Xunit;

namespace Isu.Extra.Test;

public class IsuExtraTest
{
    private IsuExtraService _isuExtraService = new ();
    private MegaFaculty _megaFaculty = new ("ТИНТ");
    private Ognp _ognp = new ("Анализ данных");
    private Dictionary<DayOfWeek, List<Lecture>> _lectures1 = new ();
    private Dictionary<DayOfWeek, List<Lecture>> _lectures2 = new ();

    [Fact]
    public void AddOgnpCourse()
    {
        _isuExtraService.AddMegaFaculty(_megaFaculty);
        _isuExtraService.AddOgnpCourse(_ognp, _megaFaculty);
        Assert.True(_isuExtraService.ContainsOgnp(_ognp));
    }

    [Fact]
    public void SubscribeStudentOnSpecificOgnp()
    {
        FillLecturesSchedule(_lectures1, new TimeOnly(10, 00), new TimeOnly(11, 30));
        FillLecturesSchedule(_lectures2, new TimeOnly(15, 00), new TimeOnly(16, 30));
        var megaFaculty = new MegaFaculty("Физико технический факультет");
        var flow = new Flow(1, 100, _lectures1);
        var group = new IsuExtraGroup(new GroupName("M3204"), flow);
        megaFaculty.AddFlow(flow);
        var student = new IsuExtraStudent(group, "Володя", 23, new CourseNumber(1), megaFaculty);
        var ognpFlow = new Flow(1, 100, _lectures2);
        var ognpGroup = new IsuExtraGroup(new GroupName("A3107"), ognpFlow);
        _ognp.AddFlow(ognpFlow);
        _isuExtraService.AddMegaFaculty(_megaFaculty);
        _isuExtraService.AddMegaFaculty(megaFaculty);
        _isuExtraService.AddOgnpCourse(_ognp, _megaFaculty);
        _isuExtraService.SubscribeStudentOnOgnp(student, _ognp, ognpFlow);
        Assert.Equal(_ognp, student.Ognp[0]);
        Assert.Equal(ognpGroup, student.OgnpGroups[0]);
    }

    [Fact]
    public void UnsubscribeStudentFromSpecificOgnp()
    {
        FillLecturesSchedule(_lectures1, new TimeOnly(10, 00), new TimeOnly(11, 30));
        FillLecturesSchedule(_lectures2, new TimeOnly(15, 00), new TimeOnly(16, 30));
        var megaFaculty = new MegaFaculty("Физико технический факультет");
        var flow = new Flow(1, 100, _lectures1);
        var group = new IsuExtraGroup(new GroupName("M3204"), flow);
        megaFaculty.AddFlow(flow);
        var student = new IsuExtraStudent(group, "Володя", 23, new CourseNumber(1), megaFaculty);
        var ognpFlow = new Flow(1, 100, _lectures2);
        var ognpGroup = new IsuExtraGroup(new GroupName("A3107"), ognpFlow);
        _ognp.AddFlow(ognpFlow);
        _isuExtraService.AddMegaFaculty(_megaFaculty);
        _isuExtraService.AddMegaFaculty(megaFaculty);
        _isuExtraService.AddOgnpCourse(_ognp, _megaFaculty);
        _isuExtraService.SubscribeStudentOnOgnp(student, _ognp, ognpFlow);
        _isuExtraService.UnsubscribeStudentfromOgnp(student, _ognp);
        Assert.DoesNotContain(_ognp, student.Ognp);
    }

    [Fact]
    public void GetFlowsOnOgnpCourse()
    {
        var flow = new Flow(1, 100, _lectures1);
        _isuExtraService.AddMegaFaculty(_megaFaculty);
        _isuExtraService.AddOgnpCourse(_ognp, _megaFaculty);
        _ognp.AddFlow(flow);
        Assert.Contains(flow, _isuExtraService.GetFlowsFromOgnpCourse(_ognp));
    }

    [Fact]
    public void GetStudentsFromOgnpGroup()
    {
        FillLecturesSchedule(_lectures1, new TimeOnly(10, 00), new TimeOnly(11, 30));
        FillLecturesSchedule(_lectures2, new TimeOnly(15, 00), new TimeOnly(16, 30));
        var megaFaculty = new MegaFaculty("Физико технический факультет");
        var flow = new Flow(1, 100, _lectures1);
        var group = new IsuExtraGroup(new GroupName("M3204"), flow);
        megaFaculty.AddFlow(flow);
        var student = new IsuExtraStudent(group, "Володя", 23, new CourseNumber(1), megaFaculty);
        var student2 = new IsuExtraStudent(group, "Паша", 24, new CourseNumber(1), megaFaculty);
        var student3 = new IsuExtraStudent(group, "Федор", 25, new CourseNumber(1), megaFaculty);
        var student4 = new IsuExtraStudent(group, "Гоша", 25, new CourseNumber(1), megaFaculty);
        var ognpFlow = new Flow(1, 100, _lectures2);
        var ognpGroup = new IsuExtraGroup(new GroupName("A3107"), ognpFlow);
        _ognp.AddFlow(ognpFlow);
        _isuExtraService.AddMegaFaculty(_megaFaculty);
        _isuExtraService.AddMegaFaculty(megaFaculty);
        _isuExtraService.AddOgnpCourse(_ognp, _megaFaculty);
        _isuExtraService.SubscribeStudentOnOgnp(student, _ognp, ognpFlow);
        _isuExtraService.SubscribeStudentOnOgnp(student2, _ognp, ognpFlow);
        _isuExtraService.SubscribeStudentOnOgnp(student3, _ognp, ognpFlow);
        _isuExtraService.SubscribeStudentOnOgnp(student4, _ognp, ognpFlow);
        ReadOnlyCollection<IsuExtraStudent> studentList = _isuExtraService.GetStudentsFromOgnpGroup(ognpGroup);
        Assert.Equal(student, studentList[0]);
        Assert.Equal(student2, studentList[1]);
        Assert.Equal(student3, studentList[2]);
        Assert.Equal(student4, studentList[3]);
    }

    [Fact]
    public void GetUnsubscribedStudentsFromGroup()
    {
        FillLecturesSchedule(_lectures1, new TimeOnly(10, 00), new TimeOnly(11, 30));
        FillLecturesSchedule(_lectures2, new TimeOnly(15, 00), new TimeOnly(16, 30));
        var megaFaculty = new MegaFaculty("Физико технический факультет");
        var flow = new Flow(1, 100, _lectures1);
        var group = new IsuExtraGroup(new GroupName("M3204"), flow);
        megaFaculty.AddFlow(flow);
        var student = new IsuExtraStudent(group, "Володя", 23, new CourseNumber(1), megaFaculty);
        var student2 = new IsuExtraStudent(group, "Паша", 24, new CourseNumber(1), megaFaculty);
        var student3 = new IsuExtraStudent(group, "Федор", 25, new CourseNumber(1), megaFaculty);
        var student4 = new IsuExtraStudent(group, "Гоша", 25, new CourseNumber(1), megaFaculty);
        var student5 = new IsuExtraStudent(group, "Тима", 25, new CourseNumber(1), megaFaculty);
        var ognpFlow = new Flow(1, 100, _lectures2);
        var ognpGroup = new IsuExtraGroup(new GroupName("A3107"), ognpFlow);
        _ognp.AddFlow(ognpFlow);
        _isuExtraService.AddMegaFaculty(_megaFaculty);
        _isuExtraService.AddMegaFaculty(megaFaculty);
        _isuExtraService.AddOgnpCourse(_ognp, _megaFaculty);
        _isuExtraService.SubscribeStudentOnOgnp(student, _ognp, ognpFlow);
        _isuExtraService.SubscribeStudentOnOgnp(student2, _ognp, ognpFlow);
        _isuExtraService.SubscribeStudentOnOgnp(student3, _ognp, ognpFlow);
        _isuExtraService.SubscribeStudentOnOgnp(student4, _ognp, ognpFlow);
        Assert.Contains(student5, _isuExtraService.GetUnsubscribedStudentsFromGroup(group));
    }

    private void FillLecturesSchedule(Dictionary<DayOfWeek, List<Lecture>> lectures, TimeOnly startTime, TimeOnly endTime)
    {
        var lector = new Lector("Повышев", "ЭВМ");
        var auditory = new Auditory(1123);
        var lecture = new Lecture(startTime, endTime, lector, auditory);
        for (DayOfWeek d = DayOfWeek.Monday; d != DayOfWeek.Sunday + 6; d++)
        {
            lectures[d] = new List<Lecture>();
            lectures[d].Add(lecture);
        }
    }
}