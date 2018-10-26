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
        IList<IDecisionResultItem> m_Clusters = null;
        public DecisionResult(string name) : base(name)
        {
            m_Clusters = new List<IDecisionResultItem>();
        }
        public IList<IDecisionResultItem> Clusters { get => m_Clusters; }

    }
}
