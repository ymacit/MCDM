using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Model;
using MultiCriteriaDecision.Enum;
using MultiCriteriaDecision.Helper;

namespace MultiCriteriaDecision.Analysis
{
    public class DecisionResult : ItemBase, IDecisionResult
    {
        Dictionary<IDecisionItem, float> m_Alternatives = null;
        Dictionary<IComparisonPerspective, float> m_Consistencies = null;
        public DecisionResult(string name) : base(name)
        {
            m_Alternatives = new Dictionary<IDecisionItem, float>();
            m_Consistencies = new Dictionary<IComparisonPerspective, float>();
        }
        internal void AddAlternative(IDecisionItem item, float Value)
        {
            m_Alternatives.Add(item, Value);
        }
        public IReadOnlyDictionary<IDecisionItem, float> Alternatives { get => m_Alternatives; }
        internal void AddConsistency(IComparisonPerspective item, float Value)
        {
            m_Consistencies.Add(item, Value);
        }
        public IReadOnlyDictionary<IComparisonPerspective, float> Consistencies { get => m_Consistencies; }
    }
}
