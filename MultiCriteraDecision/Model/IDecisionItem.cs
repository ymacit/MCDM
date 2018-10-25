using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Enum;

namespace MultiCriteriaDecision.Model
{
    public interface IDecisionItem
    {
        Guid ID { get; }
        string Name { get; set; }
        IDecision ParentProject { get; }
        ItemType ItemType { get; }
        IDecisionItem ParentItem { get; }
        IDecisionItem Add(ItemType type, string name);
        void Remove(IDecisionItem item);
        IReadOnlyList<IDecisionItem> Childs { get; }
    }
}
