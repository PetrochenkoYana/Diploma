using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CSML
{
    public class MatrixOfMatrix
    {
        private ArrayList Values;

        /// <summary>
        /// Number of rows of the matrix.
        /// </summary>
        public int RowCount
        {
            get { return rowCount; }
        }

        /// <summary>
        /// Number of columns of the matrix.
        /// </summary>
        public int ColumnCount
        {
            get { return columnCount; }
        }

        /// <summary>
        /// Number of rows of the matrix.
        /// </summary>
        private int rowCount;

        /// <summary>
        /// Number of columns of the matrix.
        /// </summary>
        private int columnCount;

        /// <summary>
        /// Inits empty matrix 
        /// </summary>
        public MatrixOfMatrix()
        {
            Values = new ArrayList();
            rowCount = 0;
            columnCount = 0;
        }

        /// <summary>
        /// Creates m by n matrix filled with zeros; same as Zeros(m, n).
        /// </summary>
        /// <param name="m">Number of rows</param>
        /// <param name="n">Number of columns</param>
        public MatrixOfMatrix(int m, int n)
        {
            rowCount = m;
            columnCount = n;

            Values = new ArrayList(m);

            for (int i = 0; i < m; i++)
            {
                Values.Add(new ArrayList(n));

                for (int j = 0; j < n; j++)
                {
                    ((ArrayList)Values[i]).Add(null);
                }
            }
        }
        public virtual Matrix this[int i, int j]
        {
            set
            {
                if (i <= 0 || j <= 0)
                    throw new ArgumentOutOfRangeException("Indices must be real positive.");

                if (i > rowCount)
                {
                    // dynamically add i-Rows new rows...
                    for (int k = 0; k < i - rowCount; k++)
                    {
                        this.Values.Add(new ArrayList(columnCount));

                        // ...with Cols columns
                        for (int t = 0; t < columnCount; t++)
                        {
                            ((ArrayList)Values[rowCount + k]).Add(Complex.Zero);
                        }
                    }

                    rowCount = i; // ha!
                }


                if (j > columnCount)
                {
                    // dynamically add j-Cols columns to each row
                    for (int k = 0; k < rowCount; k++)
                    {
                        for (int t = 0; t < j - columnCount; t++)
                        {
                            ((ArrayList)Values[k]).Add(Complex.Zero);
                        }
                    }

                    columnCount = j;
                }

                ((ArrayList)Values[i - 1])[j - 1] = value;
                //this.Values[i - 1, j - 1] = value; 
            }
            get
            {
                if (i > 0 && i <= rowCount && j > 0 && j <= columnCount)
                {                   

                    return (Matrix)(((ArrayList)Values[i - 1])[j - 1]);
                }
                else
                    throw new ArgumentOutOfRangeException("Indices must not exceed size of matrix.");
            }
        }
    }
}

