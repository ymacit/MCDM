using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Enum;

namespace MultiCriteriaDecision.Model
{
    public interface IDecisionItem
    {
        string Name { get; set; }
        ItemType ItemType { get; }
        IDecisionItem Parent { get; }
        IDecisionItem Add(ItemType type, string name);
        void Remove(IDecisionItem item);
        IReadOnlyList<IDecisionItem> Childs { get; }
    }
}
