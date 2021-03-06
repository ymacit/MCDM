﻿/*
    Matrix class in C#
    Written by Ivan Kuckir (ivan.kuckir@gmail.com, http://blog.ivank.net)
    http://blog.ivank.net/lightweight-matrix-class-in-c-strassen-algorithm-lu-decomposition.html
    Faculty of Mathematics and Physics Charles University in Prague (C) 2010
    - updated on 01.06.2014 - Trimming the string before parsing
    - updated on 14.06.2012 - parsing improved. Thanks to Andy!
    - updated on 03.10.2012 - there was a terrible bug in LU, SoLE and Inversion. Thanks to Danilo Neves Cruz for reporting that!
    - updated on 21.01.2014 - multiple changes based on comments -> see git for further info
	
    This code is distributed under MIT licence.
	
		Permission is hereby granted, free of charge, to any person
		obtaining a copy of this software and associated documentation
		files (the "Software"), to deal in the Software without
		restriction, including without limitation the rights to use,
		copy, modify, merge, publish, distribute, sublicense, and/or sell
		copies of the Software, and to permit persons to whom the
		Software is furnished to do so, subject to the following
		conditions:

		The above copyright notice and this permission notice shall be
		included in all copies or substantial portions of the Software.

		THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
		EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
		OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
		NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
		HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
		WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
		FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
		OTHER DEALINGS IN THE SOFTWARE.
*/

/*
 * https://en.wikipedia.org/wiki/Array_data_structure
 * http://www.mathcs.emory.edu/~cheung/Courses/561/Syllabus/3-C/array.html
 * https://eli.thegreenplace.net/2015/memory-layout-of-multi-dimensional-arrays
 * 2D row-major : [offset=i_{row}*NCOLS+i_{col}] 
 * 2D column-major :[offset=i_{col}*NROWS+i_{row}]
*/

using System;
using System.Text;
using System.Text.RegularExpressions;

namespace MultiCriteriaDecision.Helper
{
    public class Matrix
    {
        public int RowCount;
        public int ColumnCount;
        public double[] StorageArray;

        public Matrix L;
        public Matrix U;
        private int[] pi;
        private double detOfP = 1;

        public Matrix(int iRows, int iCols)         // Matrix Class constructor
        {
            RowCount = iRows;
            ColumnCount = iCols;
            StorageArray = new double[RowCount * ColumnCount];
        }

        public Matrix(double[] matrix )         // Matrix Class constructor
        {
            RowCount = 1;
            ColumnCount = matrix.Length;
            StorageArray = new double[RowCount * ColumnCount];
            matrix.CopyTo(StorageArray, 0);
        }

        public Matrix(double[,] matrix)         // Matrix Class constructor
        {
            RowCount = matrix.GetLength(0);
            ColumnCount = matrix.GetLength(1);
            StorageArray = new double[RowCount * ColumnCount];
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    StorageArray[i * ColumnCount + j] = matrix[i, j];
                }
            }
        }
        internal void ChangeDimesion(int newRowCount, int newColumnCount)
        {
            if (RowCount * ColumnCount == newRowCount * newColumnCount)
            {
                L = null;
                U = null;
                RowCount = newRowCount;
                ColumnCount = newColumnCount;
            }
            else
                throw new MException("Total dimension is different");
        }

        public int GetColumnMaxValue(int iColumn, double initialValue)
        {
            int tmp_index = -1;
            double tmp_minValue = initialValue;
            double tmp_currentValue = 0;
            for (int i = 0; i < RowCount; i++)
            {
                tmp_currentValue = StorageArray[i * ColumnCount + iColumn];
                if (tmp_currentValue > tmp_minValue)
                {
                    tmp_minValue = tmp_currentValue;
                    tmp_index = i;
                }
            }
            return tmp_index;
        }

        public int GetNegativeMinValueByRow(int iRow)
        {
            int tmp_index = -1;
            double tmp_minValue = 0;
            double tmp_currentValue = 0;
            for (int i = 0; i < ColumnCount; i++)
            {
                tmp_currentValue = StorageArray[iRow * ColumnCount + i];
                if (tmp_currentValue < tmp_minValue)
                {
                    tmp_minValue = tmp_currentValue;
                    tmp_index = i;
                }
            }
            return tmp_index;
        }

        public int GetPositiveMinValueByRow(int iRow)
        {
            int tmp_index = -1;
            double tmp_minValue = double.MaxValue;
            double tmp_currentValue = 0;
            for (int i = 0; i < ColumnCount; i++)
            {
                tmp_currentValue = StorageArray[iRow * ColumnCount + i];
                if (tmp_currentValue >= 0 && tmp_currentValue > tmp_minValue)
                {
                    tmp_minValue = tmp_currentValue;
                    tmp_index = i;
                }
            }
            return tmp_index;
        }

        public int GetPositiveMaxValueByRow(int iRow)
        {
            int tmp_index = -1;
            double tmp_maxValue = 0;
            double tmp_currentValue = 0;
            for (int i = 0; i < ColumnCount; i++)
            {
                tmp_currentValue = StorageArray[iRow * ColumnCount + i];
                if (tmp_currentValue > tmp_maxValue)
                {
                    tmp_maxValue = tmp_currentValue;
                    tmp_index = i;
                }
            }
            return tmp_index;
        }
        public int GetNegativeMaxValueByRow(int iRow)
        {
            int tmp_index = -1;
            double tmp_maxValue = 0;
            double tmp_currentValue = 0;
            for (int i = 0; i < ColumnCount; i++)
            {
                tmp_currentValue = StorageArray[iRow * ColumnCount + i];
                if (tmp_currentValue <0 && tmp_currentValue > tmp_maxValue)
                {
                    tmp_maxValue = tmp_currentValue;
                    tmp_index = i;
                }
            }
            return tmp_index;
        }

        public double GetMaxEigenvalueByWeight(Matrix wMatrix)
        {
            //A*w=λ*w => λ= (A*w)/w, λmax=Avg(λn) 
            double tmp_ResultValue = 0;
            Matrix tmp_AwMatrix = new Matrix(wMatrix.RowCount, 1);
            tmp_AwMatrix = Matrix.Multiply(this, wMatrix);

            for (int i = 0; i < tmp_AwMatrix.RowCount; i++)
            {
                tmp_ResultValue += tmp_AwMatrix[i,0]/ wMatrix[i,0];
            }

            tmp_ResultValue = tmp_ResultValue / tmp_AwMatrix.RowCount;
            return tmp_ResultValue;
        }


        public Boolean IsSquare()
        {
            return (RowCount == ColumnCount);
        }

        public double this[int iRow, int iCol]      // Access this matrix as a 2D array
        {
            get { return StorageArray[iRow * ColumnCount + iCol]; }
            set { StorageArray[iRow * ColumnCount + iCol] = value; }
        }

        public Matrix GetCol(int k)
        {
            Matrix m = new Matrix(RowCount, 1);
            for (int i = 0; i < RowCount; i++) m[i, 0] = this[i, k];
            return m;
        }

        public void SetCol(Matrix v, int k)
        {
            for (int i = 0; i < RowCount; i++) this[i, k] = v[i, 0];
        }

        public void MakeLU()                        // Function for LU decomposition
        {
            if (!IsSquare()) throw new MException("The matrix is not square!");
            L = IdentityMatrix(RowCount, ColumnCount);
            U = Duplicate();

            pi = new int[RowCount];
            for (int i = 0; i < RowCount; i++) pi[i] = i;

            double p = 0;
            double pom2;
            int k0 = 0;
            int pom1 = 0;

            for (int k = 0; k < ColumnCount - 1; k++)
            {
                p = 0;
                for (int i = k; i < RowCount; i++)      // find the row with the biggest pivot
                {
                    if (Math.Abs(U[i, k]) > p)
                    {
                        p = Math.Abs(U[i, k]);
                        k0 = i;
                    }
                }
                if (p == 0) // samé nuly ve sloupci
                    throw new MException("The matrix is singular!");

                pom1 = pi[k]; pi[k] = pi[k0]; pi[k0] = pom1;    // switch two RowCount in permutation matrix

                for (int i = 0; i < k; i++)
                {
                    pom2 = L[k, i]; L[k, i] = L[k0, i]; L[k0, i] = pom2;
                }

                if (k != k0) detOfP *= -1;

                for (int i = 0; i < ColumnCount; i++)                  // Switch RowCount in U
                {
                    pom2 = U[k, i]; U[k, i] = U[k0, i]; U[k0, i] = pom2;
                }

                for (int i = k + 1; i < RowCount; i++)
                {
                    L[i, k] = U[i, k] / U[k, k];
                    for (int j = k; j < ColumnCount; j++)
                        U[i, j] = U[i, j] - L[i, k] * U[k, j];
                }
            }
        }

        public Matrix SolveWith(Matrix v)                        // Function solves Ax = v in confirmity with solution vector "v"
        {
            if (RowCount != ColumnCount) throw new MException("The matrix is not square!");
            if (RowCount != v.RowCount) throw new MException("Wrong number of results in solution vector!");
            if (L == null) MakeLU();

            Matrix b = new Matrix(RowCount, 1);
            for (int i = 0; i < RowCount; i++) b[i, 0] = v[pi[i], 0];   // switch two items in "v" due to permutation matrix

            Matrix z = SubsForth(L, b);
            Matrix x = SubsBack(U, z);

            return x;
        }

        // TODO check for redundancy with MakeLU() and SolveWith()
        public void MakeRref()                                    // Function makes reduced echolon form
        {
            int lead = 0;
            for (int r = 0; r < RowCount; r++)
            {
                if (ColumnCount <= lead) break;
                int i = r;
                while (this[i, lead] == 0)
                {
                    i++;
                    if (i == RowCount)
                    {
                        i = r;
                        lead++;
                        if (ColumnCount == lead)
                        {
                            lead--;
                            break;
                        }
                    }
                }
                for (int j = 0; j < ColumnCount; j++)
                {
                    double temp = this[r, j];
                    this[r, j] = this[i, j];
                    this[i, j] = temp;
                }
                double div = this[r, lead];
                for (int j = 0; j < ColumnCount; j++) this[r, j] /= div;
                for (int j = 0; j < RowCount; j++)
                {
                    if (j != r)
                    {
                        double sub = this[j, lead];
                        for (int k = 0; k < ColumnCount; k++) this[j, k] -= (sub * this[r, k]);
                    }
                }
                lead++;
            }
        }

        public Matrix Invert()                                   // Function returns the inverted matrix
        {
            if (L == null) MakeLU();

            Matrix inv = new Matrix(RowCount, ColumnCount);

            for (int i = 0; i < RowCount; i++)
            {
                Matrix Ei = Matrix.ZeroMatrix(RowCount, 1);
                Ei[i, 0] = 1;
                Matrix col = SolveWith(Ei);
                inv.SetCol(col, i);
            }
            return inv;
        }

        public Matrix Invert(bool resetLU )
        {
            L = null;
            return Invert();
        }
        public double Det()                         // Function for determinant
        {
            if (L == null) MakeLU();
            double det = detOfP;
            for (int i = 0; i < RowCount; i++) det *= U[i, i];
            return det;
        }

        public Matrix GetP()                        // Function returns permutation matrix "P" due to permutation vector "pi"
        {
            if (L == null) MakeLU();

            Matrix matrix = ZeroMatrix(RowCount, ColumnCount);
            for (int i = 0; i < RowCount; i++) matrix[pi[i], i] = 1;
            return matrix;
        }

        public Matrix Duplicate()                   // Function returns the copy of this matrix
        {
            Matrix matrix = new Matrix(RowCount, ColumnCount);
            for (int i = 0; i < RowCount; i++)
                for (int j = 0; j < ColumnCount; j++)
                    matrix[i, j] = this[i, j];
            return matrix;
        }

        public static Matrix SubsForth(Matrix A, Matrix b)          // Function solves Ax = b for A as a lower triangular matrix
        {
            if (A.L == null) A.MakeLU();
            int n = A.RowCount;
            Matrix x = new Matrix(n, 1);

            for (int i = 0; i < n; i++)
            {
                x[i, 0] = b[i, 0];
                for (int j = 0; j < i; j++) x[i, 0] -= A[i, j] * x[j, 0];
                x[i, 0] = x[i, 0] / A[i, i];
            }
            return x;
        }

        public static Matrix SubsBack(Matrix A, Matrix b)           // Function solves Ax = b for A as an upper triangular matrix
        {
            if (A.L == null) A.MakeLU();
            int n = A.RowCount;
            Matrix x = new Matrix(n, 1);

            for (int i = n - 1; i > -1; i--)
            {
                x[i, 0] = b[i, 0];
                for (int j = n - 1; j > i; j--) x[i, 0] -= A[i, j] * x[j, 0];
                x[i, 0] = x[i, 0] / A[i, i];
            }
            return x;
        }

        public static Matrix ZeroMatrix(int iRows, int iCols)       // Function generates the zero matrix
        {
            Matrix matrix = new Matrix(iRows, iCols);
            for (int i = 0; i < iRows; i++)
                for (int j = 0; j < iCols; j++)
                    matrix[i, j] = 0;
            return matrix;
        }

        public static Matrix IdentityMatrix(int iRows, int iCols)   // Function generates the identity matrix
        {
            Matrix matrix = ZeroMatrix(iRows, iCols);
            for (int i = 0; i < Math.Min(iRows, iCols); i++)
                matrix[i, i] = 1;
            return matrix;
        }

        public static Matrix RandomMatrix(int iRows, int iCols, int dispersion)       // Function generates the random matrix
        {
            Random random = new Random();
            Matrix matrix = new Matrix(iRows, iCols);
            for (int i = 0; i < iRows; i++)
                for (int j = 0; j < iCols; j++)
                    matrix[i, j] = random.Next(-dispersion, dispersion);
            return matrix;
        }

        public static Matrix Parse(string ps)                        // Function parses the matrix from string
        {
            string s = NormalizeMatrixString(ps);
            string[] RowCount = Regex.Split(s, "\r\n");
            string[] nums = RowCount[0].Split(' ');
            Matrix matrix = new Matrix(RowCount.Length, nums.Length);
            try
            {
                for (int i = 0; i < RowCount.Length; i++)
                {
                    nums = RowCount[i].Split(' ');
                    for (int j = 0; j < nums.Length; j++) matrix[i, j] = double.Parse(nums[j]);
                }
            }
            catch (FormatException) { throw new MException("Wrong input format!"); }
            return matrix;
        }

        public override string ToString()                           // Function returns matrix as a string
        {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                    s.Append(String.Format("{0,5:E2}", this[i, j]) + " ");
                s.AppendLine();
            }
            return s.ToString();
        }

        public static Matrix Transpose(Matrix m)              // Matrix transpose, for any rectangular matrix
        {
            Matrix t = new Matrix(m.ColumnCount, m.RowCount);
            for (int i = 0; i < m.RowCount; i++)
                for (int j = 0; j < m.ColumnCount; j++)
                    t[j, i] = m[i, j];
            return t;
        }

        public static Matrix Power(Matrix m, int pow)           // Power matrix to exponent
        {
            if (pow == 0) return IdentityMatrix(m.RowCount, m.ColumnCount);
            if (pow == 1) return m.Duplicate();
            if (pow == -1) return m.Invert();

            Matrix x;
            if (pow < 0) { x = m.Invert(); pow *= -1; }
            else x = m.Duplicate();

            Matrix ret = IdentityMatrix(m.RowCount, m.ColumnCount);
            while (pow != 0)
            {
                if ((pow & 1) == 1) ret *= x;
                x *= x;
                pow >>= 1;
            }
            return ret;
        }

        private static void SafeAplusBintoC(Matrix A, int xa, int ya, Matrix B, int xb, int yb, Matrix C, int size)
        {
            for (int i = 0; i < size; i++)          // RowCount
                for (int j = 0; j < size; j++)     // ColumnCount
                {
                    C[i, j] = 0;
                    if (xa + j < A.ColumnCount && ya + i < A.RowCount) C[i, j] += A[ya + i, xa + j];
                    if (xb + j < B.ColumnCount && yb + i < B.RowCount) C[i, j] += B[yb + i, xb + j];
                }
        }

        private static void SafeAminusBintoC(Matrix A, int xa, int ya, Matrix B, int xb, int yb, Matrix C, int size)
        {
            for (int i = 0; i < size; i++)          // RowCount
                for (int j = 0; j < size; j++)     // ColumnCount
                {
                    C[i, j] = 0;
                    if (xa + j < A.ColumnCount && ya + i < A.RowCount) C[i, j] += A[ya + i, xa + j];
                    if (xb + j < B.ColumnCount && yb + i < B.RowCount) C[i, j] -= B[yb + i, xb + j];
                }
        }

        private static void SafeACopytoC(Matrix A, int xa, int ya, Matrix C, int size)
        {
            for (int i = 0; i < size; i++)          // RowCount
                for (int j = 0; j < size; j++)     // ColumnCount
                {
                    C[i, j] = 0;
                    if (xa + j < A.ColumnCount && ya + i < A.RowCount) C[i, j] += A[ya + i, xa + j];
                }
        }
        public static void CopyAtoB(Matrix A, int aRow, int aCol, Matrix B, int bRow, int bCol, int sizeRow, int sizeCol)
        {
            for (int i = 0; i < sizeRow; i++)          // RowCount
                for (int j = 0; j < sizeCol; j++)     // ColumnCount
                {
                    //B[i, j] = 0;
                    //if (aRow + j < A.RowCount && aCol + i < A.ColumnCount)
                    B[i+bRow, j+bCol] += A[aRow + i, aCol + j];
                }
        }

        private static void AplusBintoC(Matrix A, int xa, int ya, Matrix B, int xb, int yb, Matrix C, int size)
        {
            for (int i = 0; i < size; i++)          // RowCount
                for (int j = 0; j < size; j++) C[i, j] = A[ya + i, xa + j] + B[yb + i, xb + j];
        }

        private static void AminusBintoC(Matrix A, int xa, int ya, Matrix B, int xb, int yb, Matrix C, int size)
        {
            for (int i = 0; i < size; i++)          // RowCount
                for (int j = 0; j < size; j++) C[i, j] = A[ya + i, xa + j] - B[yb + i, xb + j];
        }

        private static void ACopytoC(Matrix A, int xa, int ya, Matrix C, int size)
        {
            for (int i = 0; i < size; i++)          // RowCount
                for (int j = 0; j < size; j++) C[i, j] = A[ya + i, xa + j];
        }

        // TODO assume matrix 2^N x 2^N and then directly call StrassenMultiplyRun(A,B,?,1,?)
        private static Matrix StrassenMultiply(Matrix A, Matrix B)                // Smart matrix multiplication
        {
            if (A.ColumnCount != B.RowCount) throw new MException("Wrong dimension of matrix!");

            Matrix R;

            int msize = Math.Max(Math.Max(A.RowCount, A.ColumnCount), Math.Max(B.RowCount, B.ColumnCount));

            int size = 1; int n = 0;
            while (msize > size) { size *= 2; n++; };
            int h = size / 2;


            Matrix[,] mField = new Matrix[n, 9];

            /*
             *  8x8, 8x8, 8x8, ...
             *  4x4, 4x4, 4x4, ...
             *  2x2, 2x2, 2x2, ...
             *  . . .
             */

int z;
            for (int i = 0; i < n - 4; i++)          // RowCount
            {
                z = (int)Math.Pow(2, n - i - 1);
                for (int j = 0; j < 9; j++) mField[i, j] = new Matrix(z, z);
            }

            SafeAplusBintoC(A, 0, 0, A, h, h, mField[0, 0], h);
            SafeAplusBintoC(B, 0, 0, B, h, h, mField[0, 1], h);
            StrassenMultiplyRun(mField[0, 0], mField[0, 1], mField[0, 1 + 1], 1, mField); // (A11 + A22) * (B11 + B22);

            SafeAplusBintoC(A, 0, h, A, h, h, mField[0, 0], h);
            SafeACopytoC(B, 0, 0, mField[0, 1], h);
            StrassenMultiplyRun(mField[0, 0], mField[0, 1], mField[0, 1 + 2], 1, mField); // (A21 + A22) * B11;

            SafeACopytoC(A, 0, 0, mField[0, 0], h);
            SafeAminusBintoC(B, h, 0, B, h, h, mField[0, 1], h);
            StrassenMultiplyRun(mField[0, 0], mField[0, 1], mField[0, 1 + 3], 1, mField); //A11 * (B12 - B22);

            SafeACopytoC(A, h, h, mField[0, 0], h);
            SafeAminusBintoC(B, 0, h, B, 0, 0, mField[0, 1], h);
            StrassenMultiplyRun(mField[0, 0], mField[0, 1], mField[0, 1 + 4], 1, mField); //A22 * (B21 - B11);

            SafeAplusBintoC(A, 0, 0, A, h, 0, mField[0, 0], h);
            SafeACopytoC(B, h, h, mField[0, 1], h);
            StrassenMultiplyRun(mField[0, 0], mField[0, 1], mField[0, 1 + 5], 1, mField); //(A11 + A12) * B22;

            SafeAminusBintoC(A, 0, h, A, 0, 0, mField[0, 0], h);
            SafeAplusBintoC(B, 0, 0, B, h, 0, mField[0, 1], h);
            StrassenMultiplyRun(mField[0, 0], mField[0, 1], mField[0, 1 + 6], 1, mField); //(A21 - A11) * (B11 + B12);

            SafeAminusBintoC(A, h, 0, A, h, h, mField[0, 0], h);
            SafeAplusBintoC(B, 0, h, B, h, h, mField[0, 1], h);
            StrassenMultiplyRun(mField[0, 0], mField[0, 1], mField[0, 1 + 7], 1, mField); // (A12 - A22) * (B21 + B22);

            R = new Matrix(A.RowCount, B.ColumnCount);                  // result

            /// C11
            for (int i = 0; i < Math.Min(h, R.RowCount); i++)          // RowCount
                for (int j = 0; j < Math.Min(h, R.ColumnCount); j++)     // ColumnCount
                    R[i, j] = mField[0, 1 + 1][i, j] + mField[0, 1 + 4][i, j] - mField[0, 1 + 5][i, j] + mField[0, 1 + 7][i, j];

            /// C12
            for (int i = 0; i < Math.Min(h, R.RowCount); i++)          // RowCount
                for (int j = h; j < Math.Min(2 * h, R.ColumnCount); j++)     // ColumnCount
                    R[i, j] = mField[0, 1 + 3][i, j - h] + mField[0, 1 + 5][i, j - h];

            /// C21
            for (int i = h; i < Math.Min(2 * h, R.RowCount); i++)          // RowCount
                for (int j = 0; j < Math.Min(h, R.ColumnCount); j++)     // ColumnCount
                    R[i, j] = mField[0, 1 + 2][i - h, j] + mField[0, 1 + 4][i - h, j];

            /// C22
            for (int i = h; i < Math.Min(2 * h, R.RowCount); i++)          // RowCount
                for (int j = h; j < Math.Min(2 * h, R.ColumnCount); j++)     // ColumnCount
                    R[i, j] = mField[0, 1 + 1][i - h, j - h] - mField[0, 1 + 2][i - h, j - h] + mField[0, 1 + 3][i - h, j - h] + mField[0, 1 + 6][i - h, j - h];

            return R;
        }
        private static void StrassenMultiplyRun(Matrix A, Matrix B, Matrix C, int l, Matrix[,] f)    // A * B into C, level of recursion, matrix field
        {
            int size = A.RowCount;
            int h = size / 2;

            AplusBintoC(A, 0, 0, A, h, h, f[l, 0], h);
            AplusBintoC(B, 0, 0, B, h, h, f[l, 1], h);
            StrassenMultiplyRun(f[l, 0], f[l, 1], f[l, 1 + 1], l + 1, f); // (A11 + A22) * (B11 + B22);

            AplusBintoC(A, 0, h, A, h, h, f[l, 0], h);
            ACopytoC(B, 0, 0, f[l, 1], h);
            StrassenMultiplyRun(f[l, 0], f[l, 1], f[l, 1 + 2], l + 1, f); // (A21 + A22) * B11;

            ACopytoC(A, 0, 0, f[l, 0], h);
            AminusBintoC(B, h, 0, B, h, h, f[l, 1], h);
            StrassenMultiplyRun(f[l, 0], f[l, 1], f[l, 1 + 3], l + 1, f); //A11 * (B12 - B22);

            ACopytoC(A, h, h, f[l, 0], h);
            AminusBintoC(B, 0, h, B, 0, 0, f[l, 1], h);
            StrassenMultiplyRun(f[l, 0], f[l, 1], f[l, 1 + 4], l + 1, f); //A22 * (B21 - B11);

            AplusBintoC(A, 0, 0, A, h, 0, f[l, 0], h);
            ACopytoC(B, h, h, f[l, 1], h);
            StrassenMultiplyRun(f[l, 0], f[l, 1], f[l, 1 + 5], l + 1, f); //(A11 + A12) * B22;

            AminusBintoC(A, 0, h, A, 0, 0, f[l, 0], h);
            AplusBintoC(B, 0, 0, B, h, 0, f[l, 1], h);
            StrassenMultiplyRun(f[l, 0], f[l, 1], f[l, 1 + 6], l + 1, f); //(A21 - A11) * (B11 + B12);

            AminusBintoC(A, h, 0, A, h, h, f[l, 0], h);
            AplusBintoC(B, 0, h, B, h, h, f[l, 1], h);
            StrassenMultiplyRun(f[l, 0], f[l, 1], f[l, 1 + 7], l + 1, f); // (A12 - A22) * (B21 + B22);

            /// C11
            for (int i = 0; i < h; i++)          // RowCount
                for (int j = 0; j < h; j++)     // ColumnCount
                    C[i, j] = f[l, 1 + 1][i, j] + f[l, 1 + 4][i, j] - f[l, 1 + 5][i, j] + f[l, 1 + 7][i, j];

            /// C12
            for (int i = 0; i < h; i++)          // RowCount
                for (int j = h; j < size; j++)     // ColumnCount
                    C[i, j] = f[l, 1 + 3][i, j - h] + f[l, 1 + 5][i, j - h];

            /// C21
            for (int i = h; i < size; i++)          // RowCount
                for (int j = 0; j < h; j++)     // ColumnCount
                    C[i, j] = f[l, 1 + 2][i - h, j] + f[l, 1 + 4][i - h, j];

            /// C22
            for (int i = h; i < size; i++)          // RowCount
                for (int j = h; j < size; j++)     // ColumnCount
                    C[i, j] = f[l, 1 + 1][i - h, j - h] - f[l, 1 + 2][i - h, j - h] + f[l, 1 + 3][i - h, j - h] + f[l, 1 + 6][i - h, j - h];
        }
        private static Matrix StupidMultiply(Matrix m1, Matrix m2)                  // Stupid matrix multiplication
        {
            if (m1.ColumnCount != m2.RowCount) throw new MException("Wrong dimensions of matrix!");

            Matrix result = ZeroMatrix(m1.RowCount, m2.ColumnCount);
            for (int i = 0; i < result.RowCount; i++)
                for (int j = 0; j < result.ColumnCount; j++)
                    for (int k = 0; k < m1.ColumnCount; k++)
                        result[i, j] += m1[i, k] * m2[k, j];
            return result;
        }

        private static Matrix Multiply(Matrix m1, Matrix m2)                         // Matrix multiplication
        {
            if (m1.ColumnCount != m2.RowCount) throw new MException("Wrong dimension of matrix!");
            int msize = Math.Max(Math.Max(m1.RowCount, m1.ColumnCount), Math.Max(m2.RowCount, m2.ColumnCount));
            // stupid multiplication faster for small matrices
            if (msize < 32)
            {
                return StupidMultiply(m1, m2);
            }
            // stupid multiplication faster for non square matrices
            if (!m1.IsSquare() || !m2.IsSquare()) {
                return StupidMultiply(m1, m2);
            }
            // Strassen multiplication is faster for large square matrix 2^N x 2^N
            // NOTE because of previous checks msize == m1.ColumnCount == m1.RowCount == m2.ColumnCount == m2.ColumnCount
            double exponent = Math.Log(msize) / Math.Log(2);
            if (Math.Pow(2,exponent) == msize) {
                return StrassenMultiply(m1, m2);
            } else {
                return StupidMultiply(m1, m2);
            }
        }
        private static Matrix Multiply(double n, Matrix m)                          // Multiplication by constant n
        {
            Matrix r = new Matrix(m.RowCount, m.ColumnCount);
            for (int i = 0; i < m.RowCount; i++)
                for (int j = 0; j < m.ColumnCount; j++)
                    r[i, j] = m[i, j] * n;
            return r;
        }
        private static Matrix Add(Matrix m1, Matrix m2)         // Sčítání matic
        {
            if (m1.RowCount != m2.RowCount || m1.ColumnCount != m2.ColumnCount) throw new MException("Matrices must have the same dimensions!");
            Matrix r = new Matrix(m1.RowCount, m1.ColumnCount);
            for (int i = 0; i < r.RowCount; i++)
                for (int j = 0; j < r.ColumnCount; j++)
                    r[i, j] = m1[i, j] + m2[i, j];
            return r;
        }

        public static string NormalizeMatrixString(string matStr)	// From Andy - thank you! :)
        {
            // Remove any multiple spaces
            while (matStr.IndexOf("  ") != -1)
                matStr = matStr.Replace("  ", " ");

            // Remove any spaces before or after newlines
            matStr = matStr.Replace(" \r\n", "\r\n");
            matStr = matStr.Replace("\r\n ", "\r\n");

            // If the data ends in a newline, remove the trailing newline.
            // Make it easier by first replacing \r\n’s with |’s then
            // restore the |’s with \r\n’s
            matStr = matStr.Replace("\r\n", "|");
            while (matStr.LastIndexOf("|") == (matStr.Length - 1))
                matStr = matStr.Substring(0, matStr.Length - 1);

            matStr = matStr.Replace("|", "\r\n");
            return matStr.Trim();
        }

        //   O P E R A T O R S

        public static Matrix operator -(Matrix m)
        { return Matrix.Multiply(-1, m); }

        public static Matrix operator +(Matrix m1, Matrix m2)
        { return Matrix.Add(m1, m2); }

        public static Matrix operator -(Matrix m1, Matrix m2)
        { return Matrix.Add(m1, -m2); }

        public static Matrix operator *(Matrix m1, Matrix m2)
        { return Matrix.Multiply(m1, m2); }

        public static Matrix operator *(double n, Matrix m)
        { return Matrix.Multiply(n, m); }
    }

    //  The class for exceptions

    public class MException : Exception
    {
        public MException(string Message)
            : base(Message)
        { }
    }
}