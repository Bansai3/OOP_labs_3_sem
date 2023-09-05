using IsuExtraException;

namespace Isu.Extra;

public class MegaFaculty
{
    private List<Flow> _flows;
    private List<Ognp> _ognp;

    public MegaFaculty(string title)
    {
        if (!CheckTitle(title)) throw new MegaFacultyTitleException("Invalid title format!");
        Title = title;
        _flows = new List<Flow>();
        _ognp = new List<Ognp>();
    }

    public string Title { get; private set; }

    public void ChangeTitle(string newTitle)
    {
        if (!CheckTitle(newTitle)) throw new MegaFacultyTitleException("Invalid title format!");
        Title = newTitle;
    }

    public void AddFlow(Flow flow)
    {
        if (!CheckFlow(flow)) throw new FlowNullReferenceException("Flow can not be null!");
        if (CheckSimilarFlow(flow)) throw new FlowNumberException($"Flow with number {flow.Number} already exists!");
        _flows.Add(flow);
    }

    public void AddOgnp(Ognp ognp)
    {
        if (!CheckOgnp(ognp)) throw new OgnpNullReferenceException("Ognp can not be null!");
        if (CheckSimilarOgnp(ognp)) throw new SimilarOgnpException($"Ognp {ognp.Title} already exists!");
        _ognp.Add(ognp);
    }

    public bool ContainsGroup(IsuExtraGroup? group)
    {
        if (group is null) return false;
        Flow? groupFlow = _flows.SingleOrDefault(fl => fl == group.Flow);
        if (groupFlow is null) return false;
        return _flows.Any(fl => fl.Groups.Any(gr => gr == group));
    }

    public Ognp? FindOgnp(Ognp ognpToFind)
    {
        if (!CheckOgnp(ognpToFind)) throw new OgnpNullReferenceException("Ognp can not be null!");
        return _ognp.SingleOrDefault(ognp => ognp.Title == ognpToFind.Title);
    }

    private bool CheckTitle(string title)
    {
        if (string.IsNullOrEmpty(title)) throw new MegaFacultyTitleException("Invalid title format!");
        string[] fullTitle = title.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return fullTitle.All(str => str.All(char.IsLetter));
    }

    private bool CheckFlow(Flow? flow) => flow is not null;

    private bool CheckSimilarFlow(Flow flow) => _flows.SingleOrDefault(fl => fl.Number == flow.Number) is not null;

    private bool CheckOgnp(Ognp? ognp) => ognp is not null;

    private bool CheckSimilarOgnp(Ognp ognp) => _ognp.SingleOrDefault(el => el.Title == ognp.Title) is not null;
}