using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Enum;

namespace MultiCriteriaDecision.Model
{
    public interface IDecisionItem : IDecisionItemBase
    {
        IDecisionItem Add(ItemType type, string name);
        void Remove(IDecisionItem item);
    }
}
