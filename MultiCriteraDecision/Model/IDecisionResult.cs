using System;
using System.Collections.Generic;
using System.Text;

namespace MultiCriteriaDecision.Model
{
    public interface IDecisionResult : IBase
    {
        IList<IDecisionResultItem> Clusters { get; }
    }
}
