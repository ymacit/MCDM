using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Enum;

namespace MultiCriteriaDecision.Model
{
    public interface IDecisionItemBase : IBase
    {
        IDecision ParentProject { get; }
        ItemType ItemType { get; }
        IDecisionItemBase ParentItem { get; }
        IReadOnlyList<IDecisionItemBase> Childs { get; }
    }
}
