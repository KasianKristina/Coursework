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

            //field.MoveDetailRight();
            //field.startKing.Move();
            field.Walls();
            field.Wave(0,4,7,4);
            field.Draw();
            Console.ReadKey();
        }

      
    }


}
