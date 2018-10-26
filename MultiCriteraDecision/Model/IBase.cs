using System;
using System.Collections.Generic;
using System.Text;

namespace MultiCriteriaDecision.Model
{
    public interface IBase
    {
        Guid ID { get; }
        string Name { get; set; }

    }
}
