using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Model;
using MultiCriteriaDecision.Enum;
using MultiCriteriaDecision.Helper;


namespace MultiCriteriaDecision.Analysis
{
    public class Judge :ItemBase, IJudge
    {
        public Judge(string name):base( name)
        {
        }
        public List<Judgment> Judgments { get; set; }
        public string Expertise { get; set; }
    }
}
