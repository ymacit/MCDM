using System;
using System.Collections.Generic;
using System.Text;

namespace MultiCriteriaDecision.Model
{
    public interface IJudge :IBase
    {
        string Expertise { get; set; }
    }
}
