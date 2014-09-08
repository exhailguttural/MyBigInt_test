using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBigIntApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            MyBigInt first = new MyBigInt("5");
            MyBigInt second = new MyBigInt("10");
            Console.WriteLine(first + second);
            Console.WriteLine(first - second);
            Console.WriteLine(second + first);
            Console.WriteLine(second - first);
            Console.ReadLine();
        }
    }
}
