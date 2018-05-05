using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw_1_1
{
    class Program
    {
        static void Main(string[] args)
        {
            const int N = 10;
            var array = new SimpleFraction[N];
            var rndNumerator = new Random();
            var rndDenominator = new Random();
            for (int i=0;i<N;i++)
            {
                array[i] = new SimpleFraction(rndNumerator.Next(-100, 100), rndDenominator.Next(1, 100));
            }
            Console.WriteLine("Original array (size = {0}):", N);
            foreach (var i in array)
                Console.Write("{0} ", i.ToString());
            Console.WriteLine();
            Array.Sort(array);
            Console.WriteLine("Sorted array (size = {0}):", N);
            foreach (var i in array)
                Console.Write("{0} ", i.ToString());
            Console.WriteLine();
            var sum = new SimpleFraction();
            foreach (var i in array)
                sum = SimpleFraction.GetSum(sum, i);
            Console.WriteLine("sum of all fractions = {0}", sum.ToString());
            var rndIndex = new Random();
            var index1 = rndIndex.Next(0, 9);
            var index2 = N - index1 - 1;
            Console.WriteLine("For example,\nresults of operation between {0}-th ({1}) and {2}-th ({3}) fractions:", index1+1, array[index1], index2+1, array[index2]);
            Console.WriteLine("Sum = {0}", SimpleFraction.GetSum(array[index1], array[index2]));
            Console.WriteLine("Difference = {0}", SimpleFraction.GetDifference(array[index1], array[index2]));
            Console.WriteLine("Composition = {0}", SimpleFraction.GetComposition(array[index1], array[index2]));
            Console.WriteLine("Quotient = {0}", SimpleFraction.GetQuotient(array[index1], array[index2]));
        }
    }

    public sealed class SimpleFraction : IComparable
    {
        private long _numerator;
        private long _denominator;

        public long Numerator => _numerator;
        public long Denominator => _denominator;

        public SimpleFraction()
        {
            _numerator = 0;
            _denominator = 1;
        }

        public SimpleFraction(int denominator):this()
        {

        }

        public SimpleFraction(int numerator, int denominator):this()
        {
            if(numerator!=0 && denominator!=0)
            {
                int divider = GetMaxCommonDivider(Math.Abs(numerator), Math.Abs(denominator));
                _numerator = (numerator / divider) * (denominator > 0 ? 1 : -1);
                _denominator = Math.Abs(denominator) / divider;
            }
        }

        private SimpleFraction(long numerator, long denominator):this()
        {
            if (numerator != 0 && denominator != 0)
            {
                long divider = GetMaxCommonDivider(Math.Abs(numerator), Math.Abs(denominator));
                _numerator = (numerator / divider) * (denominator > 0 ? 1 : -1);
                _denominator = Math.Abs(denominator) / divider;
            }
        }

        private static int GetMaxCommonDivider(int number1, int number2)
        {
            while(number1!=number2)
            {
                if (number1 > number2)
                    number1 -= number2;
                else
                    number2 -= number1;
            }
            return number1;
        }

        private static long GetMaxCommonDivider(long number1, long number2)
        {
            while (number1 != number2)
            {
                if (number1 > number2)
                    number1 -= number2;
                else
                    number2 -= number1;
            }
            return number1;
        }

        private static int GetMinCommonMultiple(int number1, int number2)
        {
            return (int)(((long)(number1 * number2)) / GetMaxCommonDivider(number1, number2));
        }

        private static long GetMinCommonMultiple(long number1, long number2)
        {
            return (number1 * number2) / GetMaxCommonDivider(number1, number2);
        }

        public static SimpleFraction GetSum(SimpleFraction fraction1, SimpleFraction fraction2)
        {
            SimpleFraction result;
            if (fraction1.Denominator == fraction2.Denominator)
                result = new SimpleFraction(fraction1.Numerator + fraction2.Numerator, fraction1.Denominator);
            else
            {
                long multiple = GetMinCommonMultiple(fraction1.Denominator, fraction2.Denominator);
                long newNumerator1 = fraction1.Numerator * (multiple / fraction1.Denominator);
                long newNumerator2 = fraction2.Numerator * (multiple / fraction2.Denominator);
                result = new SimpleFraction(newNumerator1+newNumerator2, multiple);
            }
            return result;
        }

        public static SimpleFraction GetDifference(SimpleFraction fraction1, SimpleFraction fraction2)
        {
            var temp = new SimpleFraction(-fraction2.Numerator, fraction2.Denominator);
            return SimpleFraction.GetSum(fraction1, temp);
        }

        public static SimpleFraction GetComposition(SimpleFraction fraction1, SimpleFraction fraction2)
        {
            SimpleFraction result;
            var temp1 = new SimpleFraction(fraction1.Numerator, fraction2.Denominator);
            var temp2 = new SimpleFraction(fraction2.Numerator, fraction1.Denominator);
            result = new SimpleFraction(temp1.Numerator * temp2.Numerator, temp1.Denominator * temp2.Denominator);
            return result;
        }

        public static SimpleFraction GetQuotient(SimpleFraction fraction1, SimpleFraction fraction2)
        {
            var temp = new SimpleFraction(fraction2.Denominator, fraction2.Numerator);
            return SimpleFraction.GetComposition(fraction1, temp);
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            var temp = obj as SimpleFraction;
            long newNumerator1, newNumerator2;
            if(temp.Denominator==this.Denominator)
            {
                newNumerator1 = this.Numerator;
                newNumerator2 = temp.Numerator;
            }
            else
            {
                long multiple = GetMinCommonMultiple(this.Denominator, temp.Denominator);
                newNumerator1 = this.Numerator * (multiple / this.Denominator);
                newNumerator2 = temp.Numerator * (multiple / temp.Denominator);
            }
            if (newNumerator1 > newNumerator2)
                return 1;
            else if (newNumerator1 < newNumerator2)
                return -1;
            else
                return 0;
        }

        public override string ToString()
        {
            return String.Format("{0}/{1}", _numerator, _denominator);
        }
    }
}
