using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Лабораторная
{
    class Program
    {
        static void Main(string[] args)
        {
            Analyz analyz = new Analyz("D:\\Учёба\\СПОВТ\\Лабораторная\\Text.txt");

            analyz.RecursiveDescent();


            //Console.WriteLine(analyz.Scan(false));
            //Console.WriteLine(analyz.Scan(true));
            //Console.WriteLine(analyz.Scan(true));
            //Console.WriteLine(analyz.Scan(false));
            //Console.WriteLine(analyz.Scan(false));

            Console.ReadKey();
        }
    }
}