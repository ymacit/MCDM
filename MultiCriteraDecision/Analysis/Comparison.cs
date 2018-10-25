using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Enum;
using MultiCriteriaDecision.Model;

namespace MultiCriteriaDecision.Analysis
{
    public class Comparison : IComparison
    {
        IComparisonPerspective m_Perspective = null;
        IJudgment m_Judgment = null;
        List<IPairwiseComparison> m_PairwiseList = null;

        public Comparison(IComparisonPerspective perspective, IJudgment judgment)
        {
            m_Judgment = judgment;
            m_Perspective = perspective;
            m_PairwiseList = new List<IPairwiseComparison>();
        }
        public IComparisonPerspective Perspective { get => m_Perspective; set => m_Perspective = value as ComparisonPerspective; }

        public IJudgment Judgment { get => m_Judgment; }

        public IList<IPairwiseComparison> Pairwises { get => m_PairwiseList; }
    }
}
