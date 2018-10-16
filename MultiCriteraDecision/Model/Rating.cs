using System;
using System.Collections.Generic;
using System.Text;

namespace MultiCriteriaDecision.Model
{
    public class Rating
    {
        public string Name { get; set; }
        public List<Rating> RatedRelations { get;set; }

    }
}
