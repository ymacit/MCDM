using System;
using System.Collections.Generic;
using System.Text;

namespace MultiCriteriaDecision.Enum
{
    public enum FundamentalScale : int
    {
        Equal = 1,
        Weak = 2,
        Moderate =3,
        ModeratePlus = 4,
        Strong =5,
        StrongPlus = 6,
        VeryStrong =7,
        VeryVeryStrong = 8,
        Extreme =9

        /*
          Table 1 The fundamental scale of absolute numbers
            Intensity of
            Importance
            Definition Explanation
            1 Equal importance Two activities contribute equally to the objective
            2 Weak
            3 Moderate importance Experience and judgment slightly favor one activity over another
            4 Moderate plus
            5 Strong importance Experience and judgment strongly favor one activity over another
            6 Strong plus
            7 Very strong or demonstrated importance An activity is favored very strongly over another; its dominance demonstrated in practice
            8 Very, very strong
            9 Extreme importance The evidence favoring one activity over another is of the highest possible order of affirmation 
            Reciprocals of above -  If activity i has one of the above nonzero numbers assigned to it when compared with activity j, then j has the reciprocal value when compared with i - A reasonable assumption
            Rationals Ratios - arising from the scale  - If consistency were to be forced by obtaining n numerical values to span the matrix
          */

        /*
         * BOCR
         * We refer to the four concerns collectively as BOCR merits, having used the initials of the positive ones (benefits and opportunities) before the initials of the negative ones (costs and risks)
         */
    }

    /// <summary>
    /// We refer to the four concerns collectively as BOCR merits, having used the initials of the positive ones (benefits and opportunities) before the initials of the negative ones (costs and risks)
    /// </summary>
    public enum NetworkType : int
    {
        // positive 
        Benefit = 1,
        Opportunity = 2,
        // negative
        Cost = 3, 
        Risk = 4  
    }

    public enum ItemType : int
    {
        Model = 0,
        Goal = 1,
        Criteria = 2,
        Alternative = 3,
        None=9

    }
}
