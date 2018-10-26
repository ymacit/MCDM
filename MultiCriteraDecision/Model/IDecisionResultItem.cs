using System;
using System.Collections.Generic;
using System.Text;

namespace MultiCriteriaDecision.Model
{
    public interface IDecisionResultItem :IDecisionItemBase
    {
        float Value { get; }
    }
}
