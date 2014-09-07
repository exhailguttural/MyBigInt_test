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
            MyBigInt first = new MyBigInt("-1111");
            MyBigInt second = new MyBigInt("-1111");
            Console.WriteLine(first + second);
            Console.WriteLine(first - second);
            Console.ReadLine();
        }
    }
}
