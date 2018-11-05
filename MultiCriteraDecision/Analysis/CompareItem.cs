using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Model;
using MultiCriteriaDecision.Enum;

namespace MultiCriteriaDecision.Analysis
{
    internal struct CompareItem
    {
        /*
         * For group decision making 
         * Thomas L. Saaty, Luis G. Vargas-Models, Methods, Concepts & Applications of the Analytic Hierarchy Process-Springer (2013)
         * Thomas L. Saaty, Luis G. Vargas-Decision Making With The Analytic Network Process-Springer (2013),
         * Chapter 1.10, page 23
         * Example 1.10.4.3. page 35
         * JugmentMean=Power (J1*J2*J3*J4*J5*..*Jn, 1/n)
         * Judgments = (5,5,2,4,4) , Mean =3.8073
         */
        internal string CompareGuid { get; set; }
        internal IJudge CompareJudge { get; set; }
        internal IDecisionItem ComparePivot { get; set; }
        internal IDecisionItem CompareSource { get; set; }
        internal IDecisionItem CompareTarget { get; set; }
        internal float Ratio { get; set; }        
    }
}
