using System.Collections.ObjectModel;
using Isu.Entities;
using IsuExtraException;

namespace Isu.Extra;

public class Ognp
{
    public const int MaxFlowCount = 5;
    private List<Flow> _flows;

    public Ognp(string title)
    {
        if (!CheckTitle(title)) throw new OgnpTitleException("Invalid title format!");
        Title = title;
        _flows = new List<Flow>();
    }

    public ReadOnlyCollection<Flow> Flows => new (_flows);
    public string Title { get; private set; }

    public void ChangeTitle(string newTitle)
    {
        if (!CheckTitle(newTitle)) throw new OgnpTitleException("Invalid title format!");
        Title = newTitle;
    }

    public void AddFlow(Flow flow)
    {
        if (_flows.Count == MaxFlowCount) throw new FlowNumberException("Ognp already has max flow number!");
        if (!CheckFlow(flow)) throw new FlowNullReferenceException("Flow can not be null!");
        if (CheckSimilarFlow(flow)) throw new FlowNumberException($"Flow with number {flow.Number} already exists!");
        _flows.Add(flow);
    }

    public IsuExtraGroup RegisterStudent(IsuExtraStudent student, Flow flow)
    {
        if (!CheckStudent(student)) throw new StudentNullReferenceException("Student can not be null!");
        Flow? flowToSearch = _flows.FirstOrDefault(fl => fl == flow);
        if (flowToSearch is null) throw new FlowNullReferenceException($"Ognp {Title} does not contain such flow!");
        IsuExtraGroup? group = flowToSearch.Groups.FirstOrDefault(gr => gr.Students.Count < Group.MaxStudentsAmount);
        if (group is null) throw new GroupNullReferenceException("All groups are full on this flow!");
        if (!CheckOgnpGroupPairsIntersection(group, student.Group.Pairs))
            throw new PairIntersectionException("Practices intersect with ognp pairs!");
        if (!CheckOgnpGroupPairsIntersection(group, student.Group.Flow.Lectures))
            throw new PairIntersectionException("Lectures intersect with ognp pairs!");
        group.AddStudent(student, isOgnpStudent: true);
        return group;
    }

    private bool CheckFlows(List<Flow>? flows) => flows is null;

    private bool CheckTitle(string title)
    {
        if (string.IsNullOrEmpty(title)) return false;
        string[] fullTitle = title.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return fullTitle.All(str => str.All(char.IsLetter));
    }

    private bool CheckFlow(Flow? flow) => flow is not null;

    private bool CheckSimilarFlow(Flow flow) => _flows.SingleOrDefault(fl => fl.Number == flow.Number) is not null;

    private bool CheckStudent(Student? student) => student is not null;

    private bool CheckOgnpGroupPairsIntersection<T>(IsuExtraGroup ognpGroup, IDictionary<DayOfWeek, List<T>> pairs)
    where T : Pair
    {
        for (DayOfWeek dayOfWeek = DayOfWeek.Monday; dayOfWeek != DayOfWeek.Saturday + 6; dayOfWeek++)
        {
            if (!pairs.ContainsKey(dayOfWeek)) continue;
            if (!ognpGroup.Pairs.ContainsKey(dayOfWeek)) continue;
            if (!ognpGroup.Flow.Lectures.ContainsKey(dayOfWeek)) continue;
            List<Practice> ps = ognpGroup.Pairs[dayOfWeek];
            List<Lecture> ls = ognpGroup.Flow.Lectures[dayOfWeek];
            if (ps.Any(pr => pairs[dayOfWeek].Any(p => pr.EndTime.IsBetween(p.StartTime, p.EndTime) ||
                                                       pr.StartTime.IsBetween(p.StartTime, p.EndTime))))
                return false;
            if (ls.Any(lc => pairs[dayOfWeek].Any(p => lc.EndTime.IsBetween(p.StartTime, p.EndTime) ||
                                                       lc.StartTime.IsBetween(p.StartTime, p.EndTime))))
                return false;
        }

        return true;
    }
}