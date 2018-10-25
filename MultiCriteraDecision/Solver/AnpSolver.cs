using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MultiCriteriaDecision.Helper;
using MultiCriteriaDecision.Enum;
using MultiCriteriaDecision.Model;
using MultiCriteriaDecision.Analysis;

namespace MultiCriteriaDecision.Solver
{
    internal abstract class AnpSolver : AhpSolver
    {
        protected Matrix m_LimitedMatrix = null;

        internal AnpSolver(Decision decision):base(decision)
        {
        }        
        internal override Matrix GetResult()
        {
            return null;
        }

        internal Matrix LimitedMatrix { get => m_LimitedMatrix; set => m_LimitedMatrix = value; }

    }
}
