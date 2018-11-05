using System;
using System.Collections.Generic;
using System.Text;

namespace MultiCriteriaDecision.Helper
{
    public static class Common
    {
        const int GUIDEBYTECOUNT = 16;


        public static  Guid Combine(Guid pivot, Guid source, Guid target)
        {
            //byte[] a = x.ToByteArray();
            //byte[] b = y.ToByteArray();
            //byte[] c = z.ToByteArray();
            //return new Guid(BitConverter.GetBytes(BitConverter.ToUInt64(a, 0) ^ BitConverter.ToUInt64(b, 8)).Concat(BitConverter.GetBytes(BitConverter.ToUInt64(a, 8) ^ BitConverter.ToUInt64(b, 0))).ToArray());
            byte[] resultBytes = new byte[GUIDEBYTECOUNT];
            byte[] pivotBytes = pivot.ToByteArray();
            byte[] sourceBytes = source.ToByteArray();
            byte[] targetBytes = target.ToByteArray();

            for (int i = 0; i < GUIDEBYTECOUNT; i++)
            {
                resultBytes[i] = (byte)(pivotBytes[i] ^ sourceBytes[i] ^ targetBytes[i]);
            }
            return new Guid(resultBytes);
        }
    }
}
