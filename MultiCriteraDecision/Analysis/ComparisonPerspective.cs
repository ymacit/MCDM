using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Enum;
using MultiCriteriaDecision.Model;

namespace MultiCriteriaDecision.Analysis
{
    public class ComparisonPerspective : IComparisonPerspective
    {
        IDecisionItem m_Pivot = null;
        List<IRelation> m_Relations = null;
        List<IComparisonPerspective> m_SubPerspectives = null;
        IComparisonPerspective m_parent = null;

        public ComparisonPerspective (IDecisionItem pivot, IComparisonPerspective parent)
        {
            m_parent = parent;
            m_Pivot = pivot;
            m_Relations = new List<IRelation>();
            m_SubPerspectives = new List<IComparisonPerspective>();
        }

        public NetworkType NetworkType { get; } = NetworkType.Benefit;

        public IDecisionItem Pivot { get => m_Pivot; set => m_Pivot = value as DecisionItem; }

        public IComparisonPerspective Parent { get => m_parent; }
        public IList<IRelation> Relations { get => m_Relations; }
        public IList<IComparisonPerspective> SubPerspectives { get => m_SubPerspectives; }
    }
}
