using System;
using System.Collections.Generic;
using System.Text;

namespace MultiCriteriaDecision.Model
{
    public interface IJudge
    {
        string Name { get; set; }
        Guid ID { get; set; }
    }
}
