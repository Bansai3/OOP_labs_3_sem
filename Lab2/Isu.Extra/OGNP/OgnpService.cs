using System.Collections.ObjectModel;
using Isu.Entities;
using IsuExtraException;

namespace Isu.Extra;

internal class OgnpService : IOgnpService
{
    private List<Ognp> _ognp;

    public OgnpService()
    {
        _ognp = new List<Ognp>();
    }

    public bool ContainsOgnp(Ognp? ognp)
    {
        if (ognp is null) return false;
        return _ognp.Any(el => el == ognp);
    }

    public void AddOgnpCourse(Ognp newOgnp, MegaFaculty megaFaculty)
    {
        if (megaFaculty is null) throw new MegaFacultyNullReferenceException($"Mega faculty can not be null!");
        if (!CheckOgnp(newOgnp)) throw new OgnpNullReferenceException("Ognp can not be null!");
        megaFaculty.AddOgnp(newOgnp);
        _ognp.Add(newOgnp);
    }

    public void SubscribeStudentOnOgnp(IsuExtraStudent student, Ognp ognp, Flow flow)
    {
        if (!CheckStudent(student)) throw new StudentNullReferenceException("Student can not be null!");
        if (!CheckOgnp(ognp)) throw new OgnpNullReferenceException("Ognp can not be null!");
        if (!CheckFlow(flow)) throw new FlowNullReferenceException("Flow is required!");
        student.AddOgnp(ognp, flow);
    }

    public void UnsubscribeStudentfromOgnp(IsuExtraStudent student, Ognp ognp)
    {
        if (!CheckStudent(student)) throw new StudentNullReferenceException("Student can not be null!");
        if (!CheckOgnp(ognp)) throw new OgnpNullReferenceException("Ognp can not be null!");
        student.DeleteOgnp(ognp);
    }

    public ReadOnlyCollection<Flow> GetFlowsFromOgnpCourse(Ognp ognp)
    {
        if (!CheckOgnp(ognp)) throw new OgnpNullReferenceException("Ognp can not be null!");
        if (!_ognp.Contains(ognp)) throw new OgnpTitleException($"Ognp {ognp.Title} does not exist!");
        return ognp.Flows;
    }

    public ReadOnlyCollection<IsuExtraStudent> GetStudentsFromOgnpGroup(IsuExtraGroup ognpGroup)
    {
        if (!CheckGroup(ognpGroup)) throw new OgnpGroupNullReferenceException("Ognp group can not be null!");
        return ognpGroup.Students;
    }

    public ReadOnlyCollection<IsuExtraStudent> GetUnsubscribedStudentsFromGroup(IsuExtraGroup group)
    {
        return new ReadOnlyCollection<IsuExtraStudent>(group.Where<IsuExtraStudent>(student => student.Ognp.Count == 0).ToList());
    }

    private bool CheckOgnp(Ognp? ognp) => ognp is not null;

    private bool CheckStudent(IsuExtraStudent? student) => student is not null;

    private bool CheckGroup(IsuExtraGroup? group) => group is not null;

    private bool CheckFlow(Flow? flow) => flow is not null;
}