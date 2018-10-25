using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Enum;
using MultiCriteriaDecision.Model;

namespace MultiCriteriaDecision.Analysis
{
    public class Relation : IRelation
    {
        DecisionItem m_Source = null;
        DecisionItem m_Target = null;
        IDecision m_Project = null;
        public Relation (IDecision project)
        {
            m_Project = project;
        }        
        public IDecision Project { get => m_Project; set => m_Project = value ; }
        public IDecisionItem Source { get => m_Source; set => m_Source=value as DecisionItem; }
        public IDecisionItem Target { get => m_Target; set => m_Target=value as DecisionItem; }
    }
}
