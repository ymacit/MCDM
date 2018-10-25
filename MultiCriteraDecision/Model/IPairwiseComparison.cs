using System;
using System.Collections.Generic;
using System.Text;

namespace MultiCriteriaDecision.Model
{
    public interface IPairwiseComparison
    {
        IComparison ParentComparison { get;  }        
        IRelation Relation { get;}        
        float Ratio { get; set; }
    }
}
