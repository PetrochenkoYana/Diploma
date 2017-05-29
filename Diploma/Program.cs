using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParticularDataSystem;
using InputData_Handling;
using CSML;
using Generator;
using System.IO;

namespace Diploma
{
    class Program
    {
        static void Main(string[] args)
        {
            ParticularSystem obj = new ParticularSystem();
            FileHandling fileHandle = new FileHandling(obj);
       
            //Generator.Generator.IsRight(g,obj);
            StreamWriter sw = new StreamWriter("C:/Users/Gaiduk/documents/visual studio 2015/Projects/Diploma/InputData_Handling/put.txt");
            Generator.Generator p = new Generator.Generator(obj);
            for (int i = 1; i < 16; i=i+2)
            {
                obj.D_0 *= i / obj.lambda;
                obj.D_1 *= i / obj.lambda;
                Generator.Generator g = new Generator.Generator(obj);
                var ergodicity = g.ErgodicityCondition();
                
                g.SearchForStationaryDistribution_Algoritm_1(obj, sw);
            }
            sw.Close();
        }
    }
}
