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
            MyBigInt first = new MyBigInt("123456789012345678901234567894567890345786876");
            MyBigInt second = new MyBigInt("8762349587263458972649578629456298475629834");
            Console.WriteLine(first + second);
            Console.WriteLine(first - second);
            Console.WriteLine(second - first);
            Console.WriteLine(first * second);
            Console.ReadLine();
        }
    }
}
