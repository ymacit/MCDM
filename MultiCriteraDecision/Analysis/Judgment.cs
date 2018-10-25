using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Model;
using MultiCriteriaDecision.Enum;
using MultiCriteriaDecision.Helper;


namespace MultiCriteriaDecision.Analysis
{
    public class Judgment : IJudgment
    {
        IJudge m_judge = null;
        List<IComparison> m_Comparisons = null;
        DateTime m_ComparisonDate = DateTime.MinValue;
        IDecision m_project=null;

        public Judgment (IJudge judge, IDecision project)
        {
            m_judge = judge;
            m_project = project;
            m_Comparisons = new List<IComparison>();
        }

        public IList<IComparison> Comparisons { get => m_Comparisons; }
        public IJudge Judge { get => m_judge; set => m_judge = value; }
        public IDecision ParentProject { get => m_project; }
        public DateTime ComparisonDate { get => m_ComparisonDate; set => m_ComparisonDate=value; }
    }
}
