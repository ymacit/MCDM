using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Enum;
using MultiCriteriaDecision.Model;

namespace MultiCriteriaDecision.Analysis
{
    public class PairwiseComparison : IPairwiseComparison
    {
        IComparison m_comparison = null;
        IRelation m_relation = null;
        public PairwiseComparison(IComparison comparison, IRelation relation)
        {
            m_comparison = comparison;
            m_relation = relation;
        }
        public IComparison ParentComparison { get => m_comparison; }
        public IRelation Relation { get => m_relation;}
        public float Ratio { get; set; }
    }
}
