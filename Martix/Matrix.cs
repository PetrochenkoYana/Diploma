using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSML;
namespace Martix
{
    public class Matrix
    {
        double[][] matrix;
        int size;
        public Matrix(int size)
        {
            this.size = size;

            for (int i = 0;i<size; i++)
            {
                matrix = new double[size][];
                for(int j = 0; j < size; j++) {
                    matrix[j] = new double[size];
                }
            }
        }
        public 
    }
}
