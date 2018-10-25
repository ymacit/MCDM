using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Enum;

namespace MultiCriteriaDecision.Model
{
    public interface IComparisonPerspective
    {
        NetworkType NetworkType { get; }
        IComparisonPerspective Parent { get; }
        IDecisionItem Pivot { get; }
        IList<IComparisonPerspective> SubPerspectives { get; }
        IList<IRelation> Relations { get;  }
    }
}
