using System.Collections.ObjectModel;
using System.Data;
using CustomException;
using Isu.Entities;
using Isu.Models;
using IsuExtraException;

namespace Isu.Extra;

public class IsuExtraStudent : Student
{
    public const int MaxOgnpCount = 2;
    private List<Ognp> _ognp;
    private List<Group> _ognpGroups;
    public IsuExtraStudent(IsuExtraGroup group, string name, int id, CourseNumber courseNumber, MegaFaculty megaFaculty)
    : base(group, name, id, courseNumber)
    {
        if (!CheckIsuExtraGroup(group)) throw new GroupNullReferenceException("Group is required!");
        Group = group;
        Group.AddStudent(this);
        if (!CheckMegaFaculty(megaFaculty)) throw new MegaFacultyNullReferenceException("Mega faculty can not be equal null!");
        if (!megaFaculty.ContainsGroup(group)) throw new GroupNullReferenceException("Mega faculty does not contain such group!");
        MegaFaculty = megaFaculty;
        _ognp = new List<Ognp>();
        _ognpGroups = new List<Group>();
    }

    public MegaFaculty MegaFaculty { get; private set; }
    public ReadOnlyCollection<Ognp> Ognp => new (_ognp);
    public ReadOnlyCollection<Group> OgnpGroups => new (_ognpGroups);
    public new IsuExtraGroup Group { get; private set; }

    public void AddOgnp(Ognp ognp, Flow flow)
    {
        if (!CheckOgnp(ognp)) throw new OgnpNullReferenceException("Ognp can not be equal null!");
        if (_ognp.Count == MaxOgnpCount) throw new OgnpCountException($"Ognps are more than {MaxOgnpCount}!");
        if (CheckSimilarOgnp(ognp)) throw new OgnpTitleException($"Ognp {ognp.Title} already exists!");
        if (IsMegaFacultyOgnp(ognp)) throw new MegaFacultyOgnpException("Student can not enrol on this ognp!");
        IsuExtraGroup? ognpGroup = ognp.RegisterStudent(this, flow);
        if (ognpGroup is null) throw new OgnpPlacesException($"There are no places on ognp {ognp.Title}");
        _ognpGroups.Add(ognpGroup);
        _ognp.Add(ognp);
    }

    public void DeleteOgnp(Ognp ognp)
    {
        if (!CheckOgnp(ognp)) throw new OgnpNullReferenceException("Ognp can not be equal null!");
        _ognpGroups[_ognp.IndexOf(ognp)].DeleteStudent(this);
        _ognpGroups.RemoveAt(_ognp.IndexOf(ognp));
        _ognp.Remove(ognp);
    }

    private bool CheckMegaFaculty(MegaFaculty? megaFaculty) => megaFaculty is not null;

    private bool CheckOgnp(Ognp? ognp) => ognp is not null;

    private bool CheckSimilarOgnp(Ognp ognp) => _ognp.SingleOrDefault(el => el.Title == ognp.Title) is not null;

    private bool IsMegaFacultyOgnp(Ognp ognp) => MegaFaculty.FindOgnp(ognp) is not null;

    private bool CheckIsuExtraGroup(IsuExtraGroup? isuExtraGroup) => isuExtraGroup is not null;
}