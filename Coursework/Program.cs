using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classes;

namespace Coursework
{
    class Program
    {
        static void Main(string[] args)
        {
            DynamicField field = new DynamicField();
            
            field.Walls();
            field.Strategy1();
            field.Draw();

            //Test.checkfun();
            Console.ReadKey();
        }
    }
}
