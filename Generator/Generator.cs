using CSML;
using ParticularDataSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Generator
{ 
    public class Generator
    {
        public Matrix Q_00;
        public Matrix Q_01;
        public Matrix Q_10;
        public Matrix Q_0;
        public Matrix Q_1;
        public Matrix Q_2;
        public Generator(ParticularSystem obj)
        {
            #region lambda 
            var D = obj.D_0 + obj.D_1;
            for (int i = 1; i <= D.RowCount; i++)
            {
                D[i, D.ColumnCount] = new Complex(1);
            }

            var b = new Matrix(1, D.RowCount);
            b[1, D.RowCount] = new Complex(1);
            var result = b * Matrix.CastingToMatrix(Matrix.reverse(D.CastingToDouble()));
            var result2 = result * obj.D_1;
            obj.lambda = (result2 * Matrix.Ones(result2.ColumnCount, 1))[1,1];
            #endregion
            #region h
            var H = obj.H_0 + obj.H_1;
            for (int i = 1; i <= H.RowCount; i++)
            {
                H[i, H.ColumnCount] = new Complex(1);
            }

            var vector = new Matrix(1, H.RowCount);
            vector[1, H.RowCount] = new Complex(1);
            var res = b * H.Inverse();
            var res2 = res * obj.H_1;
            obj.h = (res2 * Matrix.Ones(res2.ColumnCount, 1))[1, 1];
            #endregion
            var b11 = obj.beta_1 * (-obj.S_1).Inverse();
            obj.b1 = (b11 * Matrix.Ones(b11.ColumnCount, 1))[1,1];
            obj.mu_1 = new Complex(Math.Pow(obj.b1.Re, -1));
            var b22 = obj.beta_2 * (-obj.S_2).Inverse();
            obj.b2 = (b22 * Matrix.Ones(b22.ColumnCount, 1))[1, 1];
            obj.mu_2 = new Complex(Math.Pow(obj.b2.Re, -1));
            var t11 = obj.tau_1 * (-obj.T_1).Inverse();
            obj.t1 = (t11 * Matrix.Ones(t11.ColumnCount, 1))[1, 1];
            obj.nu_1 = new Complex(Math.Pow(obj.t1.Re, -1));

            var s = obj.S_1 & obj.S_2;
            obj.S_0 = -(s) * Matrix.Ones(s.ColumnCount, 1);
            obj.T_0_1 = -obj.T_1 * Matrix.Ones(obj.T_1.ColumnCount, 1);
            obj.T_0_2 = -obj.T_2 * Matrix.Ones(obj.T_2.ColumnCount, 1);
            obj.S_0_1 = -obj.S_1 * Matrix.Ones(obj.S_1.ColumnCount, 1);
            obj.S_0_2 = -obj.S_2 * Matrix.Ones(obj.S_2.ColumnCount, 1);
            int v_ = Convert.ToInt32(obj.v.Re) + 1;
            int w_ = Convert.ToInt32(obj.w.Re) + 1;
            int R_1 = Convert.ToInt32(obj.R_1.Re);
            int R_2 = Convert.ToInt32(obj.R_2.Re);
            int M_1 = Convert.ToInt32(obj.M_1.Re);
            int M_2 = Convert.ToInt32(obj.M_2.Re);
            obj.H = obj.H_0 + obj.H_1;
            var d = (Matrix.Identity(w_) % ((1 - obj.p) * obj.H_1)) % obj.tau_2;
            #region Q_00
            var block_00_0 = obj.D_0 & obj.H_0;
            var block_00_1 = (Matrix.Identity(w_) % (obj.p * obj.H_1)) % obj.tau_1;
            var block_00_2 = (Matrix.Identity(w_) % ((1 - obj.p) * obj.H_1)) % obj.tau_2;
            var block_00_3 = Matrix.Identity((w_) * (v_)) % obj.T_0_1;
            var block_00_4 = (obj.D_0 & obj.H) & obj.T_1;
            var block_00_6 = Matrix.Identity((w_) * (v_)) % obj.T_0_2;
            var blocks_00_8 = (obj.D_0 & obj.H) & obj.T_2;
            this.Q_00 = FillInBlockMatrix(block_00_0, block_00_1, block_00_2, block_00_3, block_00_4,
                Matrix.Zeros(block_00_3.RowCount, block_00_2.ColumnCount), block_00_6,
                Matrix.Zeros(block_00_6.RowCount, block_00_1.ColumnCount), blocks_00_8
                );
            #endregion

            #region Q_01
            var block_01_0 = ((obj.D_1 % Matrix.Identity(v_)) % obj.beta_1) % obj.beta_2;
            var block_01_1 = ((obj.D_1 % Matrix.Identity(v_)) % obj.beta_2) % Matrix.Identity(R_1);
            var block_01_2 = ((obj.D_1 % Matrix.Identity(v_)) % obj.beta_1) % Matrix.Identity(R_2);
            this.Q_01 = GenerateDiagonalMatrix(block_01_0, block_01_1, block_01_2);
            #endregion

            #region Q_10
            var block_10_0 = Matrix.Identity((w_) * (v_)) % obj.S_0;
            var block_10_1 = Matrix.Identity((w_) * (v_)) % obj.S_0_2 % Matrix.Identity(R_1);
            var block_10_2 = Matrix.Identity((w_) * (v_)) % obj.S_0_1 % Matrix.Identity(R_2);
            this.Q_10 = GenerateDiagonalMatrix(block_10_0, block_10_1, block_10_2);
            #endregion

            #region Q_0
            var r = obj.beta_1 % obj.beta_2;
            var block_0_0 = Matrix.Identity((w_) * (v_)) % (obj.S_0 * r);
            var block_0_1 = (Matrix.Identity((w_) * (v_)) % (obj.S_0_2 * obj.beta_2)) % Matrix.Identity(R_1);
            var block_0_2 = (Matrix.Identity((w_) * (v_)) % (obj.S_0_1 * obj.beta_1)) % Matrix.Identity(R_2);
            this.Q_0 = GenerateDiagonalMatrix(block_0_0, block_0_1, block_0_2);
            #endregion

            #region Q_1
            var block_1_0 = obj.D_0 & obj.H_0 & obj.S_1 & obj.S_2;
            var block_1_1 = (Matrix.Identity(w_) % (obj.p * obj.H_1)) % Matrix.Ones(M_1, 1)
                % Matrix.Identity(M_2) % obj.tau_1;
            var block_1_2 = (Matrix.Identity(w_) % ((1 - obj.p) * obj.H_1)) % Matrix.Identity(M_1)
                % Matrix.Ones(M_2, 1) % obj.tau_2;
            var block_1_3 = Matrix.Identity((w_) * (v_)) % obj.beta_1 % Matrix.Identity(M_2) % obj.T_0_1;
            var block_1_4 = obj.D_0 & obj.H & obj.S_2 & obj.T_1;
            var block_1_6 = Matrix.Identity((w_) * (v_)) % Matrix.Identity(M_1) % obj.beta_2 % obj.T_0_2;
            var blocks_1_8 = obj.D_0 & obj.H & obj.S_1 & obj.T_2;
            this.Q_1 = FillInBlockMatrix(block_1_0, block_1_1, block_1_2, block_1_3, block_1_4,
                Matrix.Zeros(block_1_3.RowCount, block_1_2.ColumnCount), block_1_6,
                Matrix.Zeros(block_1_6.RowCount, block_1_1.ColumnCount), blocks_1_8
                );
            #endregion

            #region Q_2
            var block_2_0 = obj.D_1 % Matrix.Identity(v_ * M_1 * M_2);
            var block_2_1 = obj.D_1 % Matrix.Identity(v_ * M_2 * R_1);
            var block_2_2 = obj.D_1 % Matrix.Identity(v_ * M_1 * R_2);
            this.Q_2 = GenerateDiagonalMatrix(block_2_0, block_2_1, block_2_2);
            #endregion

        }
        public static Matrix FillInBlockMatrix(params Matrix[] blocks)
        {
            Matrix resultMatrix = null;
            if (blocks.Length == 9)
            {
                var C = new MatrixOfMatrix(9, 1);

                for (int i = 0; i <= 6; i = i + 3)
                {
                    C[i + 1, 1] = Matrix.VerticalConcat(Matrix.VerticalConcat(blocks[i], blocks[i + 1]), blocks[i + 2]);

                }
                resultMatrix = Matrix.HorizontalConcat(Matrix.HorizontalConcat(C[1, 1], C[4, 1]), C[7, 1]);
                //for (int i = 2; i <= C.RowCount - 1; i++)
                //{
                //    resultMatrix = Matrix.HorizontalConcat(resultMatrix, C[i + 1, 1]);
                //}
            }

            if (blocks.Length == 12)
            {
                var C = new MatrixOfMatrix(14, 1);

                for (int i = 0; i < 12; i = i + 4)
                {
                    C[i + 1, 1] = Matrix.VerticalConcat(Matrix.VerticalConcat(Matrix.VerticalConcat(blocks[i], blocks[i + 1]), blocks[i + 2]), blocks[i + 3]);

                }
                resultMatrix = Matrix.HorizontalConcat(Matrix.HorizontalConcat(C[1, 1], C[5, 1]), C[9, 1]);
                //for (int i = 2; i <= C.RowCount - 1; i++)
                //{
                //    resultMatrix = Matrix.HorizontalConcat(resultMatrix, C[i + 1, 1]);
                //}
            }
            return resultMatrix;
        }
        public static Matrix GenerateDiagonalMatrix(params Matrix[] blocks)
        {
            Matrix result = null;
            if (blocks.Length == 3)
            {
                result = Generator.FillInBlockMatrix(blocks[0],
                    Matrix.Zeros(blocks[0].RowCount, blocks[1].ColumnCount),
                    Matrix.Zeros(blocks[0].RowCount, blocks[2].ColumnCount),
                    Matrix.Zeros(blocks[1].RowCount, blocks[0].ColumnCount),
                    blocks[1],
                    Matrix.Zeros(blocks[1].RowCount, blocks[2].ColumnCount),
                    Matrix.Zeros(blocks[2].RowCount, blocks[0].ColumnCount),
                    Matrix.Zeros(blocks[2].RowCount, blocks[1].ColumnCount),
                    blocks[2]
                    );
            }
            return result;
        }
        public static void IsRight(Generator g,ParticularSystem obj)
        {
            FileStream fs = new FileStream("C:/Users/Gaiduk/documents/visual studio 2015/Projects/Diploma/InputData_Handling/Test.txt", FileMode.Create);
            // First, save the standard output.
            TextWriter tmp = Console.Out;
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);
            Console.WriteLine("lambda "+$"{obj.lambda}");
            Console.WriteLine("h " + $"{obj.h}");
            Console.WriteLine("b1 " + $"{obj.b1}");
            Console.WriteLine("mu_1 " + $"{obj.mu_1}");
            Console.WriteLine("b2 " + $"{obj.b2}");
            Console.WriteLine("mu_2 " + $"{obj.mu_2}");
            Console.WriteLine("t1 " + $"{obj.t1}");
            Console.WriteLine("nu_1 " + $"{obj.nu_1}");
            var result = FillInBlockMatrix(g.Q_00, g.Q_01, Matrix.Zeros(g.Q_00.RowCount, g.Q_2.ColumnCount), Matrix.Zeros(g.Q_00.RowCount, g.Q_2.ColumnCount),
                g.Q_10, g.Q_1, g.Q_2, Matrix.Zeros(g.Q_10.RowCount, g.Q_2.ColumnCount), Matrix.Zeros(g.Q_0.RowCount, g.Q_10.ColumnCount), g.Q_0, g.Q_1, g.Q_2);
            Complex[] sum = new Complex[120];
            for (int i = 0; i < sum.Length; i++)
                sum[i] = new Complex();
            for (int i = 1; i < 116; i++)
            {
                for (int j = 1; j <= result.ColumnCount; j++)
                {
                    sum[i] += result[i, j];
                }
                Console.WriteLine($"{i} " + sum[i]);
            }
            var r = Matrix.VerticalConcat(g.Q_00, g.Q_01);
            //Complex[] sum = new Complex[100];
            //for (int i = 0; i < sum.Length; i++)
            //    sum[i] = new Complex();
            //for (int i = 1; i < r.RowCount; i++)
            //{
            //    for (int j = 1; j < r.ColumnCount; j++)
            //    {
            //        sum[i] += r[i, j];
            //    }
            //    Console.WriteLine(sum[i]);
            //}
            var p = r * Matrix.Ones(r.ColumnCount, 1);
        }
        public bool ErgodicityCondition()
        {
            var A = this.Q_0 + this.Q_1 + this.Q_2;
            for (int i = 1; i <= A.RowCount; i++)
            {
                A[i, A.ColumnCount] = new Complex(1);
            }
          
            var b = new Matrix(1, A.RowCount);
            b[1, A.RowCount] = new Complex(1);
            var p= Matrix.reverse(A.CastingToDouble());
            var w = A * Matrix.CastingToMatrix(p);
            var result = b * Matrix.CastingToMatrix(p);

            var condition1 = result * this.Q_2;
            var condition2 = result * this.Q_0;
            var d = condition1 * Matrix.Ones(condition1.ColumnCount, 1);
            var t = condition2 * Matrix.Ones(condition2.ColumnCount, 1);
            if ((condition1 * Matrix.Ones(condition1.ColumnCount, 1))[1, 1] < (condition2 * Matrix.Ones(condition2.ColumnCount, 1))[1, 1])
            {
                return true;
            }
            else
                return false;
        }
        //    public Matrix SearchForStationaryDistribution()
        //    {
        //        var R = new Matrix(this.Q_0.RowCount, this.Q_0.ColumnCount);
        //        var IterationR = R + (R ^ 2) * this.Q_0 + R * this.Q_1 + this.Q_2;
        //        while ((IterationR - R).MaxNorm() < 0.00000001)
        //        {
        //            R = IterationR;
        //            IterationR= R + (R^2) * this.Q_0 + R * this.Q_1 + this.Q_2;
        //        }
        //        var system1=this.Q_1+this.Q_10(-this.Q_00)
        //    }
        //}
    }
}
