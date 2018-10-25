using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Enum;

namespace MultiCriteriaDecision.Model
{
    public interface IRelation
    {
        IDecision Project { get;  }
        IDecisionItem Source { get; set;}
        IDecisionItem Target { get; set;}
    }
}
