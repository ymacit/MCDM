using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Enum;

namespace MultiCriteriaDecision.Model
{
    public class Relation : IRelation
    {
        DecisionItem m_Source = null;
        DecisionItem m_Target = null;


        public NetworkType NetworkType { get; } = NetworkType.Benefit;
        public IDecisionItem Source { get => m_Source; set => m_Source=value as DecisionItem; }
        public IDecisionItem Target { get => m_Target; set => m_Target=value as DecisionItem; }
    }
}
