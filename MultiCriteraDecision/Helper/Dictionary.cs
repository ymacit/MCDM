using System;
using System.Collections.Generic;
using System.Text;

namespace MultiCriteriaDecision.Helper
{
    public static class Dictionary
    {
        public static SortedList<int, Single> ConsistencyRatio = null;

        private static SortedList<int, Single> ConsistencyRatio_Fill()
        {
            SortedList<int, Single> keyValuePairs = new SortedList<int, Single>();
            /*
             * The Analytic Network Process
             * Thomas L. Saaty
             * University of Pittsburgh
             *
             * Table 2.Random Index
             *      ---------------------------------------------------------------------------------
             *Order |1   |2   |3   |4   |5   |6   |7   |8   |9    |10   |11   |12   |13   |14  |15  |
             *R.I.  |0   |0   |0.52|0.89|1.11|1.25|1.35|1.40|1.45 |1.49 |1.51 |1.54 |1.56 |1.57|1.58|
             *λmax  |0.0 |0.0 |0.05|4.21|5.44|6.62|7.81|8.98|10.16|11.34|12.51|13.69|14.87|16.0|17.2|
             *      ---------------------------------------------------------------------------------
             *      C.I. = (λmax −n)/(n−1)
            */
            keyValuePairs.Add(1, 0);
            keyValuePairs.Add(2, 0);
            keyValuePairs.Add(3, 0.52f);
            keyValuePairs.Add(4, 0.89f);
            keyValuePairs.Add(5, 1.11f);
            keyValuePairs.Add(6, 1.25f);
            keyValuePairs.Add(7, 1.35f);
            keyValuePairs.Add(8, 1.40f);
            keyValuePairs.Add(9, 1.45f);
            keyValuePairs.Add(10, 1.49f);
            keyValuePairs.Add(11, 1.51f);
            keyValuePairs.Add(12, 1.54f);
            keyValuePairs.Add(13, 1.56f);
            keyValuePairs.Add(14, 1.57f);
            keyValuePairs.Add(15, 1.58f);
            return keyValuePairs;
        }
    }
}
