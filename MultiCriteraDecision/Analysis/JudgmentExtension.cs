using System;
using System.Collections.Generic;
using System.Text;
using MultiCriteriaDecision.Helper;
using MultiCriteriaDecision.Model;

namespace MultiCriteriaDecision.Analysis
{
    public static class JudgmentExtension
    {

        internal static float GetGroupRating(this Judgment item,  List<CompareItem> compares)
        {
            double tmp_rating = 1;
            int tmp_ratingCount = 0;
            foreach (CompareItem compare in compares)
            {
                if (compare.Ratio > 0)
                {
                    tmp_rating *= compare.Ratio;
                    tmp_ratingCount++;
                }
            }
            if (tmp_ratingCount > 0)
                tmp_rating = Math.Pow(tmp_rating, (1 / tmp_ratingCount));
            else
                tmp_rating = 0;

            return (float)tmp_rating;
        }
    }
}
