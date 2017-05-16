using ParticularDataSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSML;
using System.Globalization;

namespace InputData_Handling
{
    public class FileHandling
    {
        public FileHandling(ParticularSystem obj)
        {
            ReadingData(obj);
        }
        public static void ReadingData(ParticularSystem obj)
        {
            using (StreamReader stream = new StreamReader("C:/Users/Gaiduk/documents/visual studio 2015/Projects/Diploma/InputData_Handling/InputData.txt"))
            {
                var inputLine = stream.ReadToEnd().Split('|','\r','\n');
                var str = inputLine.Where(s=>s!="").ToArray();
                Func<int,int> DoubleIncrement = delegate ( int a) { return a+2; };
                int i = 0;
                obj.v = new Complex(Convert.ToDouble(str[++i], CultureInfo.InvariantCulture));
                i = DoubleIncrement(i);
                obj.w = new Complex(Convert.ToDouble(str[i], CultureInfo.InvariantCulture));
                i = DoubleIncrement(i);
                obj.R_1= new Complex(Convert.ToDouble(str[i], CultureInfo.InvariantCulture));
                i = DoubleIncrement(i);
                obj.R_2 = new Complex(Convert.ToDouble(str[i], CultureInfo.InvariantCulture));
                i = DoubleIncrement(i);
                obj.M_1 = new Complex(Convert.ToDouble(str[i], CultureInfo.InvariantCulture));
                i = DoubleIncrement(i);
                obj.M_2 = new Complex(Convert.ToDouble(str[i], CultureInfo.InvariantCulture));
                i = DoubleIncrement(i);
                obj.D_0 = new Matrix(str[i]);
                obj.D_0 = obj.D_0 * 0.5;
                i = DoubleIncrement(i);
                obj.D_1 = new Matrix(str[i]);
                obj.D_1 = obj.D_1 * 0.5;
                i = DoubleIncrement(i);
                obj.H_0= new Matrix(str[i]);
                i = DoubleIncrement(i);
                obj.H_1 = new Matrix(str[i]);
                i = DoubleIncrement(i);
                obj.beta_1 = new Matrix(str[i]);
                i = DoubleIncrement(i);
                obj.beta_2 = new Matrix(str[i]);
                i = DoubleIncrement(i);
                obj.S_1 = new Matrix(str[i]);
                i = DoubleIncrement(i);
                obj.S_2 = new Matrix(str[i]);
                i = DoubleIncrement(i);
                obj.tau_1 = new Matrix(str[i]);
                i = DoubleIncrement(i);
                obj.tau_2 = new Matrix(str[i]);
                i = DoubleIncrement(i);
                obj.T_1 = new Matrix(str[i]);
                i = DoubleIncrement(i);
                obj.T_2 = new Matrix(str[i]);
                i = DoubleIncrement(i);
                obj.p = new Complex(Convert.ToDouble(str[i],CultureInfo.InvariantCulture));
            }
        }
    }
}
