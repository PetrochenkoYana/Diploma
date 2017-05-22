using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParticularDataSystem;
using InputData_Handling;
using CSML;
using Generator;

namespace Diploma
{
    class Program
    {
        static void Main(string[] args)
        {
            ParticularSystem obj = new ParticularSystem();
            FileHandling fileHandle = new FileHandling(obj);
            Generator.Generator g = new Generator.Generator(obj);
            Generator.Generator.IsRight(g,obj);
            var ergodicity=g.ErgodicityCondition();
            g.SearchForStationaryDistribution_Algoritm_1(obj);
            Console.ReadKey();
        }
    }
}
