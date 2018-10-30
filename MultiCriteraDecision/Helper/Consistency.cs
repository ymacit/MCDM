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

        public static float Check(Matrix ComparisonMatrix, Matrix WeigtMatrix)
        {
            int tmp_Count = WeigtMatrix.RowCount;
            float tmp_MaxEigen = (float)ComparisonMatrix.GetMaxEigenvalueByWeight(WeigtMatrix);
            float tmp_ConsistencyRatio = (tmp_MaxEigen - tmp_Count) / (tmp_Count - 1);
            float tmp_RandomIndex = DecisionDictionary.ConsistencyRatio[tmp_Count];
            tmp_ConsistencyRatio = tmp_ConsistencyRatio / tmp_RandomIndex;
            return (float)tmp_ConsistencyRatio;
        }
    }
}
