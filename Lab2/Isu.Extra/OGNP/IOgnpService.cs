using System.Collections.ObjectModel;

namespace Isu.Extra;

public interface IOgnpService
{
    void AddOgnpCourse(Ognp newOgnp, MegaFaculty megaFaculty);
    void SubscribeStudentOnOgnp(IsuExtraStudent student, Ognp ognp, Flow flow);
    void UnsubscribeStudentfromOgnp(IsuExtraStudent student, Ognp ognp);
    ReadOnlyCollection<Flow> GetFlowsFromOgnpCourse(Ognp ognp);
    ReadOnlyCollection<IsuExtraStudent> GetStudentsFromOgnpGroup(IsuExtraGroup ognpGroup);
    ReadOnlyCollection<IsuExtraStudent> GetUnsubscribedStudentsFromGroup(IsuExtraGroup group);
}