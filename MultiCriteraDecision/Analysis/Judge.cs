using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Model;
using MultiCriteriaDecision.Enum;
using MultiCriteriaDecision.Helper;


namespace MultiCriteriaDecision.Analysis
{
    public class Judge :IJudge
    {
        public Judge()
        {
            ID = Guid.NewGuid();
        }
        public string Name { get; set; }
        public Guid ID { get; set; }
        public List<Judgment> Judgments { get; set; }
    }
}
