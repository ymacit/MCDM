using System;
using System.Collections.Generic;
using System.Text;

namespace MultiCriteriaDecision.Model
{
    public interface IDecisionResult : IBase
    {
        IReadOnlyDictionary<IDecisionItem, float> Alternatives { get; }        
        IReadOnlyDictionary<IComparisonPerspective, float> Consistencies { get; }
    }
}
