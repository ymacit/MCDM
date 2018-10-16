using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Enum;

namespace MultiCriteriaDecision.Model
{
    public interface IRelation
    {
        NetworkType NetworkType { get; }
        IDecisionItem Source { get; set;}
        IDecisionItem Target { get; set;}
    }
}
