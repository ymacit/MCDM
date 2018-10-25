using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Enum;

namespace MultiCriteriaDecision.Helper
{
    public static class Consistency
    {
        //saaty 
        private const double ReferenceConsistencyRatio= 0.10;

        public static double Check(Matrix ComparisonMatrix, Matrix WeigtMatrix)
        {
            int tmp_Count = WeigtMatrix.RowCount;
            double tmp_MaxEigen = ComparisonMatrix.GetMaxEigenvalueByWeight(WeigtMatrix);
            double tmp_ConsistencyRatio = (tmp_MaxEigen - tmp_Count) / (tmp_Count - 1);
            double tmp_RandomIndex = DecisionDictionary.ConsistencyRatio[tmp_Count];
            tmp_ConsistencyRatio = tmp_ConsistencyRatio / tmp_RandomIndex;
            return tmp_ConsistencyRatio;
        }
    }
}
