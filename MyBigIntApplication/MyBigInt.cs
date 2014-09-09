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
        public int Sign {get; set;}
        private static int radix = 10000;

        public MyBigInt(String value)
        {
            Sign = 1;
            if (value.ElementAt(0) == '-' || value.ElementAt(0) == '+')
            {
                if (value.ElementAt(0) == '-')
                {
                    Sign = -1;
                }
                value = value.Substring(1, value.Length - 1);
            }    
            Factors = new int[(int)Math.Ceiling((double)value.Length / 4)];
            int i = value.Length - 1;
            int j = 0;
            if (checkInputString(value))
            {
                try
                {
                    for ( ; i >= 0; i -= 4)
                    {
                        int parseTokenLength = Math.Min(i + 1, 4);
                        Factors[j] = int.Parse(value.Substring(i - parseTokenLength + 1, parseTokenLength));
                        j++;
                    }
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("exc");
                }
            }
        }

        private MyBigInt(int[] factors, int sign)
        {
            Sign = sign;
            Factors = factors;
        }

        public static MyBigInt operator +(MyBigInt a, MyBigInt b) {
            List<int> resultFactorsList = new List<int>();
            int rest = 0;
            int factorsSum = 0;
            bool signsChanged = false;
            int aSign = a.Sign;
            int bSign = b.Sign;
            if (MyBigInt.checkIfNegativeIsBigger(a, b))
            {
                aSign = -aSign;
                bSign = -bSign;
                signsChanged = true;
            }
            for (int i = 0; (i < Math.Max(a.Factors.Length, b.Factors.Length)) || (i > 0 && rest != 0); i++)
            {
                factorsSum += a.Factors.Length - 1 >= i ? a.Factors[i] * aSign : 0;
                factorsSum += b.Factors.Length - 1 >= i ? b.Factors[i] * bSign : 0;
                factorsSum += rest;
                if (factorsSum < 0 && i < Math.Max(a.Factors.Length, b.Factors.Length) - 1 && aSign != bSign)
                {
                    factorsSum += radix;
                    rest = -1;
                } else
                if (Math.Abs(factorsSum) > radix)
                {
                    rest = factorsSum / radix;
                    factorsSum = factorsSum % radix;
                }
                else rest = 0;
                resultFactorsList.Add(Math.Abs(factorsSum));
                factorsSum = 0;
            }
            int resultSign = aSign == bSign && bSign < 0 ? -1 : 1;
            MyBigInt result = new MyBigInt(resultFactorsList.ToArray(), resultSign);
            if (signsChanged)
            {
                result.Sign = -result.Sign;
            }
            return result;
        }

        public static MyBigInt operator -(MyBigInt a, MyBigInt b)
        {
            return a + new MyBigInt(b.Factors, -b.Sign);
        }

        public static MyBigInt operator *(MyBigInt a, MyBigInt b)
        {
            List<int> resultFactorsList = new List<int>();
            int factorProduct;
            int rest = 0;
            for (int i = 0; i < b.Factors.Length; i++)
            {
                for (int j = 0; j < a.Factors.Length || rest != 0; j++)
                {
                    if (j >= a.Factors.Length) factorProduct = rest;
                    else factorProduct = a.Factors[j] * b.Factors[i] + rest;
                    if (factorProduct > radix)
                    {
                        rest = factorProduct / radix;
                        factorProduct = factorProduct % radix;
                    }
                    else rest = 0;
                    resultFactorsList.Add(factorProduct);
                }
            }
            return new MyBigInt(resultFactorsList.ToArray(), a.Sign * b.Sign);
        }

        public override String ToString()
        {
            StringBuilder stringBuild = new StringBuilder();
            if (Sign < 0) stringBuild.Append('-');
            for (int i = Factors.Length - 1; i >= 0; i--)
            {
                stringBuild.Append(Factors[i]);
            }
                return stringBuild.ToString();
        }

        private bool checkInputString(String value)
        {
            int startIndex = value.ElementAt(0) == '-' || value.ElementAt(0) == '+' ? 1 : 0;
            String pattern = "\\D";
            if (Regex.IsMatch(value.Substring(startIndex, value.Length - startIndex), pattern)) return false;
            else return true;
        }

        public static bool checkIfNegativeIsBigger(MyBigInt a, MyBigInt b)
        {
            if (a.Sign == b.Sign && b.Sign < 0) return false;
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
                    if (a.Factors[i] > b.Factors[i])
                    {
                        theBiggest = a;
                        break;
                    }
                    else if (a.Factors[i] < b.Factors[i])
                    {
                        theBiggest = b;
                        break;
                    }
                }
            }
            if (theBiggest != null && theBiggest.Sign < 0) return true;
            else return false;
        }
    }
}
