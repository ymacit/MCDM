using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Enum;

namespace MultiCriteriaDecision.Model
{
    public interface IDecisionItem : IBase
    {
        IDecision ParentProject { get; }
        ItemType ItemType { get; }
        IDecisionItem ParentItem { get; }
        IReadOnlyList<IDecisionItem> Childs { get; }
        IDecisionItem Add(ItemType type, string name);
        void Remove(IDecisionItem item);
    }
}
