using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MATH_2
{
    public class Iccheslenie
    {
        public Iccheslenie(double bas)
        {
            Base = bas;
        }
        public double Base;
        public string value;       
        public static double To10(string val, double bas)
        {
            var sum = 0.0;
            for (int i = 1; i < val.Length + 1; i++)
            {
                var pow = Math.Pow(bas, val.Length - i);
                var number = 0;
                if (val[i - 1] == 'a')
                {
                    number = 10;
                }
                else if (val[i - 1] == 'b')
                {
                    number = 11;
                }
                else if (val[i - 1] == 'c')
                {
                    number = 12;
                }
                else if (val[i - 1] == 'd')
                {
                    number = 13;
                }
                else if (val[i - 1] == 'e')
                {
                    number = 14;
                }
                else if (val[i - 1] == 'f')
                {
                    number = 15;
                }
                else
                {
                    number = int.Parse(val[i - 1].ToString());
                }
                sum += number * pow;
            }
            return sum;
        }
        public static double operator +(Iccheslenie ic, Iccheslenie ic2)
        {
            var icTo10 = To10(ic.value, ic.Base);
            var ic2To10 = To10(ic2.value, ic2.Base);
            var sum = ic2To10 + icTo10;
            return sum;
        }
        public static double operator *(Iccheslenie ic, Iccheslenie ic2)
        {
            var icTo10 = To10(ic.value, ic.Base);
            var ic2To10 = To10(ic2.value, ic2.Base);
            var sum = ic2To10 * icTo10;
            return sum;
        }
        public static double operator -(Iccheslenie ic, Iccheslenie ic2)
        {
            var icTo10 = To10(ic.value, ic.Base);
            var ic2To10 = To10(ic2.value, ic2.Base);
            var sum = icTo10 - ic2To10;
            return sum;
        }
        public static double operator /(Iccheslenie ic, Iccheslenie ic2)
        {
            var icTo10 = To10(ic.value, ic.Base);
            var ic2To10 = To10(ic2.value, ic2.Base);
            var sum = icTo10 / ic2To10;
            return sum;
        }
        public static bool operator >(Iccheslenie ic, Iccheslenie ic2)
        {
            var icTo10 = To10(ic.value, ic.Base);
            var ic2To10 = To10(ic2.value, ic2.Base);
            if (icTo10 > ic2To10)
            {
                return true;
            }
            return false;
        }
        public static bool operator <(Iccheslenie ic, Iccheslenie ic2)
        {
            var icTo10 = To10(ic.value, ic.Base);
            var ic2To10 = To10(ic2.value, ic2.Base);
            if (icTo10 < ic2To10)
            {
                return true;
            }
            return false;
        }
        public static bool operator <=(Iccheslenie ic, Iccheslenie ic2)
        {
            var icTo10 = To10(ic.value, ic.Base);
            var ic2To10 = To10(ic2.value, ic2.Base);
            if (icTo10 <= ic2To10)
            {
                return true;
            }
            return false;
        }
        public static bool operator >=(Iccheslenie ic, Iccheslenie ic2)
        {
            var icTo10 = To10(ic.value, ic.Base);
            var ic2To10 = To10(ic2.value, ic2.Base);
            if (icTo10 >= ic2To10)
            {
                return true;
            }
            return false;
        }
        public static double operator %(Iccheslenie ic, Iccheslenie ic2)
        {
            var icTo10 = To10(ic.value, ic.Base);
            var ic2To10 = To10(ic2.value, ic2.Base);
            return icTo10 % ic2To10;
        }
    }
    public class Expanenta
    {
        double BaseNumber;
        double Value;
        public Expanenta(double value)
        {
            var log = Math.Floor(Math.Log10(value));
            BaseNumber = log;
            Value = value / Math.Pow(10, log);
        }
        public double GetNumber()
        {
            return Value * Math.Pow(10, BaseNumber);
        }
        public override string ToString()
        {
            var str = "";
            str += Value.ToString() + "e" + "+" + BaseNumber.ToString();
            return str;
        }
        public static double Recognize(string str)
        {
            string[] strs = new string[1];
            strs[0] = "e";
            string[] strs2 = new string[1];
            strs2[0] = "+";
            var output = 0.0;
            var output2 = 0.0;
            var tryParse = double.TryParse(str.Split(strs, StringSplitOptions.RemoveEmptyEntries)[0], out output);
            if (!tryParse)
            {
                throw new ArgumentException("You entered not string in not correct form");
            }
            var parse2 = double.TryParse(str.Split(strs2, StringSplitOptions.RemoveEmptyEntries)[1], out output2);
            if (!parse2)
            {
                throw new ArgumentException("You entered not string in not correct form");
            }
            return output * Math.Pow(10, output2);
        }
        public static double operator +(Expanenta ex1, Expanenta ex2)
        {
            var double1 = Math.Pow(10, ex1.BaseNumber) * ex1.Value;
            var double2 = Math.Pow(10, ex2.BaseNumber) * ex2.Value;
            return double1 + double2;
        }
        public static double operator -(Expanenta ex1, Expanenta ex2)
        {
            var double1 = Math.Pow(10, ex1.BaseNumber) * ex1.Value;
            var double2 = Math.Pow(10, ex2.BaseNumber) * ex2.Value;
            return double1 - double2;
        }
        public static double operator *(Expanenta ex1, Expanenta ex2)
        {
            var double1 = Math.Pow(10, ex1.BaseNumber) * ex1.Value;
            var double2 = Math.Pow(10, ex2.BaseNumber) * ex2.Value;
            return double1 * double2;
        }
        public static double operator /(Expanenta ex1, Expanenta ex2)
        {
            var double1 = Math.Pow(10, ex1.BaseNumber) * ex1.Value;
            var double2 = Math.Pow(10, ex2.BaseNumber) * ex2.Value;
            return double1 / double2;
        }
        public static bool operator >(Expanenta ex1, Expanenta ex2)
        {
            var double1 = Math.Pow(10, ex1.BaseNumber) * ex1.Value;
            var double2 = Math.Pow(10, ex2.BaseNumber) * ex2.Value;
            if (double1 > double2)
            {
                return true;
            }
            return false;
        }
        public static bool operator <(Expanenta ex1, Expanenta ex2)
        {
            var double1 = Math.Pow(10, ex1.BaseNumber) * ex1.Value;
            var double2 = Math.Pow(10, ex2.BaseNumber) * ex2.Value;
            if (double1 < double2)
            {
                return true;
            }
            return false;
        }
        public static bool operator >=(Expanenta ex1, Expanenta ex2)
        {
            var double1 = Math.Pow(10, ex1.BaseNumber) * ex1.Value;
            var double2 = Math.Pow(10, ex2.BaseNumber) * ex2.Value;
            if (double1 >= double2)
            {
                return true;
            }
            return false;
        }
        public static bool operator <=(Expanenta ex1, Expanenta ex2)
        {
            var double1 = Math.Pow(10, ex1.BaseNumber) * ex1.Value;
            var double2 = Math.Pow(10, ex2.BaseNumber) * ex2.Value;
            if (double1 <= double2)
            {
                return true;
            }
            return false;
        }
        public static double operator %(Expanenta ex1, Expanenta ex2)
        {
            var double1 = Math.Pow(10, ex1.BaseNumber) * ex1.Value;
            var double2 = Math.Pow(10, ex2.BaseNumber) * ex2.Value;
            return double1 % double2;
        }
    }
    public class SimpleFactors
    {
        List<double> Factors;
        public SimpleFactors(List<double> factors)
        {
            Factors = factors;
        }
        public override string ToString()
        {
            var factors2 = new List<int>();
            foreach(var f in Factors)
            {
                factors2.Add((int)f);
            }            
            List<int> uniqueFactors = new List<int>();
            foreach (var factor in factors2)
            {
                if (uniqueFactors.IndexOf(factor) == -1)
                {
                    uniqueFactors.Add(factor);
                }
            }
            List<int> repeatings = new List<int>();
            var index = 0;
            foreach (var uniquefactor in uniqueFactors)
            {
                repeatings.Add(0);
                foreach (var factor in Factors)
                {
                    if (factor == uniquefactor)
                    {
                        repeatings[index] += 1;
                    }
                }
                index++;
            }
            var str = "";
            var index2 = 0;
            foreach (var uniqueFactor in uniqueFactors)
            {
                str += uniqueFactor.ToString() + "^" + repeatings[index2];
                if (index2 < uniqueFactors.Count - 1)
                {
                    str += " * ";
                }
                index2++;
            }
            return str;
        }
    }
    public static class Math2
    {
        public static readonly double PI = Math.Floor(Math.PI * 10000) / 10000;
        public static readonly double E = Math.Floor(Math.E * 10000) / 10000;
        public static double Floor(double input)
        {
            var result = input%1.0;            
            return input - result;
        }
        public static double Ceil(double input, int decimalCount)
        {
            var pow = Math.Pow(10, decimalCount - 1);
            var inp = Floor(input * pow);
            var _decimal = input * pow - inp;
            if (_decimal > 0.5)
            {
                var floor = Math.Floor(pow * 10 * input);
                floor += 1;
                return floor / (pow * 10);
            }
            else
            {
                var floor = Math.Floor(pow * input);
                return floor / pow;
            }
        }
        public static List<double> FindSimpleNumbers(double Max)
        {
            var list = new List<double>();
            for (int i = 2; i < Max; i++)
            {
                list.Add(i);
            }
            for (int i = 2; i < Max / 2; i++)
            {
                for (int index = 0; index < list.Count; index++)
                {
                    var item = list[index];
                    if (item % i == 0 && item != i)
                    {
                        list.RemoveAt(index);
                    }
                }
            }
            return list;
        }
        public static double GetAverageNumber(params double[] nums)
        {
            var numbers = nums.ToList();
            var sum = 0.0;
            foreach (var number in numbers)
            {
                sum += number;
            }
            return sum / (double)numbers.Count;
        }
        public static double GetBiggestNumber(params double[] nums)
        {
            var numbers = nums.ToList();
            var max = 0.0;
            foreach (var number in numbers)
            {
                if (number > max)
                {
                    max = number;
                }
            }
            return max;
        }
        public static double ToRadians(double Angle)
        {
            return Angle / 180 * Math.PI;
        }
        public static double GetLowestNumber(params double[] nums)
        {
            var numbers = nums.ToList();
            var max = GetBiggestNumber(nums);
            var min = max;
            foreach (var number in numbers)
            {
                if (number < min)
                {
                    min = number;
                }
            }
            return min;
        }
        public static List<double> ToSimpleFactors(double value)
        {
            var simpleNumbers = FindSimpleNumbers(value);
            var Value = value;
            List<double> factors = new List<double>();
            for (; Value != 1;)
            {
                foreach (var item in simpleNumbers)
                {
                    if (Value % item == 0)
                    {
                        factors.Add(item);
                        Value /= item;
                        break;
                    }
                }
            }
            return factors;
        }
        public static double GetDispersia(params double[] nums)
        {
            var averageNumber = GetAverageNumber(nums);
            var deviations = new List<double>();
            foreach (var number in nums.ToList())
            {
                deviations.Add(number - averageNumber);
            }
            var sum = 0.0;
            foreach (var deviation in deviations)
            {
                sum += Math.Pow(deviation, 2.0);
            }
            return sum / deviations.Count;
        }
        public static string Exp(double value)
        {
            var exp = new Expanenta(value);
            return exp.ToString();
        }
        public static double GetRandom(double min, double max, int numbersAfterComma)
        {
            var max1 = max - 1;
            var rand = new Random();
            var dou = rand.NextDouble();
            var pow = Math.Pow(10, numbersAfterComma);
            var dou2 = Math.Floor((dou * pow)) / pow;
            var number = rand.Next((int)min, (int)max) + dou2;
            return number;
        }
        public static double GetVolumeOfCylinder(double height, double area)
        {
            return height * area;
        }
        public static double GetVolumeOfCylinder(double height, double radius, double PI = Math.PI)
        {
            var area = GetAreaOfArc(radius);
            return height * area;
        }
        public static double GetAreaOfArc(double Radius, double Pi = Math.PI)
        {
            return Pi * Radius * Radius;
        }
        public static string ToHex(int Number, double baseOf)
        {
            var str = "";
            var pows = new List<double>();
            var log = Math.Floor(Math.Log(Number, baseOf));
            for (var i = log; i > -1; i--)
            {
                pows.Add(Math.Pow(baseOf, (double)i));
            }
            foreach (var number in pows)
            {
                var del = Math.Floor(Number / number);
                if (del == 10)
                {
                    str += "a";
                }
                if (del == 15)
                {
                    str += "f";
                }
                if (del == 11)
                {
                    str += "b";
                }
                if (del == 12)
                {
                    str += "c";
                }
                if (del == 13)
                {
                    str += "d";
                }
                if (del == 14)
                {
                    str += "e";
                }
                if (del < 10)
                {
                    str += del.ToString();
                }
                Number -= (int)del * (int)number;
            }
            return str;
        }        
        public static double Factorial(double number)
        {
            var factorial = 1.0;
             for (var i = 1.0; i <= number; i++)
            {
                factorial *= i;
            }
            return (double)factorial;
        }
    }    
}
