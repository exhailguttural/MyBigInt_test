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

        public MyBigInt (String value)
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
            List<int> factorsList = new List<int>();
            int i = value.Length - 1;
            if (checkInputString(value))
            {
                try
                {
                    for ( ; i >= 0; i -= 4)
                    {
                        int parseTokenLength = Math.Min(i + 1, 4);
                        factorsList.Add(int.Parse(value.Substring(i - parseTokenLength + 1, parseTokenLength)));
                    }
                    Factors = removeUnnecessaryZeros(factorsList);
                }
                catch (ArgumentException e)
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }

        private MyBigInt(int[] factors, int sign)
        {
            Sign = sign;
            Factors = removeUnnecessaryZeros(factors);
        }

        public static MyBigInt operator +(MyBigInt a, MyBigInt b) {
            List<int> resultFactorsList = new List<int>();
            int rest = 0;
            int factorsSum = 0;
            bool signsChanged = false;
            int aSign = a.Sign;
            int bSign = b.Sign;
            if (MyBigInt.checkIfNegativeIsBigger(a, b) == 0)
            {
                return new MyBigInt("0");
            }
            if (MyBigInt.checkIfNegativeIsBigger(a, b) < 0)
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
            MyBigInt result = new MyBigInt("0");
            int factorProduct;
            int rest = 0;
            for (int i = 0; i < b.Factors.Length; i++)
            {
                List<int> resultFactorsList = new List<int>();
                for (int j = 1; j <= i; j++)
                {
                    resultFactorsList.Add(0);
                }
                for (int k = 0; k < a.Factors.Length || rest != 0; k++)
                {
                    if (k >= a.Factors.Length) factorProduct = rest;
                    else factorProduct = a.Factors[k] * b.Factors[i] + rest;
                    if (factorProduct > radix)
                    {
                        rest = factorProduct / radix;
                        factorProduct = factorProduct % radix;
                    }
                    else rest = 0;
                    resultFactorsList.Add(factorProduct);
                }
                result += new MyBigInt(resultFactorsList.ToArray(), 1);
            }
            result.Sign = a.Sign * b.Sign;
            return result;
        }

        public override String ToString()
        {
            StringBuilder stringBuild = new StringBuilder();
            if (Sign < 0) stringBuild.Append('-');
            for (int i = Factors.Length - 1; i >= 0; i--)
            {
                if (i < Factors.Length - 1)
                {
                    int zerosNumber = 1;
                    while (Factors[i] / (int)Math.Pow(10, zerosNumber) != 0)
                    {
                        zerosNumber++;
                    }
                    for (int j = 0; j < 4 - zerosNumber; j++) stringBuild.Append("0");
                }
                stringBuild.Append(Factors[i]);
            }
                return stringBuild.ToString();
        }

        private bool checkInputString(String value)
        {
            String pattern = "\\D";
            if (Regex.IsMatch(value.Substring(0, value.Length), pattern) && value.Length > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static int checkIfNegativeIsBigger(MyBigInt a, MyBigInt b) 
        {                                                                   
            if (a.Sign == b.Sign && b.Sign < 0) return 1;
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
            if (theBiggest == null)
            {
                return 0;
            }
            else if (theBiggest.Sign < 0)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        private int[] removeUnnecessaryZeros(List<int> list)
        {
            return removeUnnecessaryZeros(list.ToArray());
        }

        private int[] removeUnnecessaryZeros(int[] arr)
        {
            if (arr.Length == 1) return arr;
            int newEndIndex = arr.Length - 1;
            for (int i = newEndIndex; i >= 0; i--)
            {
                if (arr[i] == 0)
                {
                    newEndIndex = i;
                }
                else
                {
                    break;
                }
            }
            return arr.Take(newEndIndex + 1).ToArray<int>();
        }
    }
}
