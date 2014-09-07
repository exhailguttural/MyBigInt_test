using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBigIntApplication
{
    class MyBigInt
    {
        public int[] Factors {get; set;}
        public bool IsPositive {get; set;}
        private static int radix = 10000;

        public MyBigInt(String value)
        {
            IsPositive = true;
            int border = 0;
            if (value.StartsWith("-") || value.StartsWith("+"))
            {
                if (value.StartsWith("-")) IsPositive = false;
                Factors = new int[(int)Math.Ceiling((double)(value.Length - 1) / 4)];
                border = 1;
            }
            else
            {
                Factors = new int[(int)Math.Ceiling((double)value.Length / 4)];
            }
            int i = value.Length - 1;
            int j = 0;
            try
            {
                for ( ; i >= border; i -= 4)
                {
                    int parseTokenLength = Math.Min(i + 1, 4);
                    Factors[j] = IsPositive ? Math.Abs(int.Parse(value.Substring(i - parseTokenLength + 1, parseTokenLength)))
                                            : -1 * Math.Abs(int.Parse(value.Substring(i - parseTokenLength + 1, parseTokenLength)));
                    j++;
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("exc");
            }
        }

        private MyBigInt(int[] factors)
        {
            if (factors[factors.Length - 1] < 0) IsPositive = false;
            Factors = factors;
        }

        public static MyBigInt operator +(MyBigInt a, MyBigInt b) {
            List<int> resultFactorsList = new List<int>();
            int t = 0;
            int factorsSum = 0;
            if (MyBigInt.checkIfNegativeIsBigger(a, b))
            {
                a.changeSign();
                b.changeSign();
            }
            for (int i = 0; (i < Math.Max(a.Factors.Length, b.Factors.Length)) || (i > 0 && t != 0); i++)
            {
                factorsSum += a.Factors.Length - 1 >= i ? a.Factors[i] : 0;
                factorsSum += b.Factors.Length - 1 >= i ? b.Factors[i] : 0;
                factorsSum += t;
                if (factorsSum < 0 && i < Math.Max(a.Factors.Length, b.Factors.Length) - 1)
                {
                    factorsSum += radix;
                    t = -1;
                } else
                if (factorsSum > radix)
                {
                    t = factorsSum / radix;
                    factorsSum = factorsSum % radix;
                }
                else t = 0;
                resultFactorsList.Add(factorsSum);
                factorsSum = 0;
            }
            MyBigInt result = new MyBigInt(resultFactorsList.ToArray());
            if (MyBigInt.checkIfNegativeIsBigger(a, b))
            {
                result.changeSign();
            }
            return result;
        }

        public static MyBigInt operator -(MyBigInt a, MyBigInt b)
        {
            b.changeSign();
            return a - b;
        }

        public override String ToString()
        {
            StringBuilder stringBuild = new StringBuilder();
            for (int i = Factors.Length - 1; i >= 0; i--)
            {
                stringBuild.Append(Factors[i]);
            }
                return stringBuild.ToString();
        }

        private bool checkInputString(String value)
        {
            int startIndex = value.StartsWith("-") || value.StartsWith("+") ? 1 : 0;
            String pattern = "\\D";
            if (Regex.IsMatch(value.Substring(startIndex, value.Length - startIndex), pattern)) return false;
            else return true;
        }

        private void changeSign()
        {
            IsPositive = !IsPositive;
            for (int i = 0; i < Factors.Length; i++)
            {
                Factors[i] = -Factors[i];
            }
        }

        public static bool checkIfNegativeIsBigger(MyBigInt a, MyBigInt b)
        {
            MyBigInt theBiggest = null;
            if (a.Factors.Length > b.Factors.Length)
            {
                theBiggest = a;
            }
            else if (a.Factors.Length < b.Factors.Length)
            {
                theBiggest = b;
            }
            else if (a.Factors.Length == b.Factors.Length)
            {
                for (int i = a.Factors.Length - 1; i >= 0; i--)
                {
                    if (Math.Abs(a.Factors[i]) > Math.Abs(b.Factors[i]))
                    {
                        theBiggest = a;
                        break;
                    }
                    else if (Math.Abs(a.Factors[i]) < Math.Abs(b.Factors[i]))
                    {
                        theBiggest = b;
                        break;
                    }
                }
            }
            if (theBiggest != null && !theBiggest.IsPositive) return true;
            else return false;
        }
    }
}
