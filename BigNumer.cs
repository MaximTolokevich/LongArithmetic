using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace arithmeticOperationsWithVeryLargeNumbers
{
    public class BigNumber
    {
        public enum Mark
        {
            negative = 0,
            positive = 1
        }
        public Mark mark;
        List<int> digits = new List<int>();
        public BigNumber()
        {

        }
        public BigNumber(string bigNumber)
        {
            if (IsPositive(bigNumber))
            {
                mark = Mark.positive;
            }
            else
            {
                mark = Mark.negative;
            }
            ParseFromStringToBigNumber(bigNumber);
        }
        public BigNumber(List<int> list)
        {
            mark = Mark.positive;
            digits = list.ToList();
        }
        public BigNumber(Mark mark, List<int> result)
        {
            this.mark = mark;
            digits = result;
        }

        public static BigNumber Zero = new BigNumber("0");
        static bool IsPositive(string number)
        {

            if (string.IsNullOrEmpty(number))
            {
                throw new ArgumentException();
            }
            if (number.StartsWith("-"))
            {

                return false;
            }
            else
            {
                return true;
            }
        }
        public static BigNumber Sum(BigNumber a, BigNumber b)
        {
            List<int> result = new List<int>();
            var maxLengthOfDigit = Math.Max(a.digits.Count, b.digits.Count);
            var minLengthOfDigit = Math.Min(a.digits.Count, b.digits.Count);
            int carryover = 0;
            var mark = Mark.positive;
            var compareResult = CompareDigitsBySize(a, b);
            // (a+ & b +) || (a- & b-)
            if (a.mark == Mark.positive && b.mark == Mark.positive || a.mark == Mark.negative && b.mark == Mark.negative)
            {
                if (a.mark == Mark.negative || b.mark == Mark.negative)
                {
                    mark = Mark.negative;

                    for (var i = 0; i < minLengthOfDigit; i++)
                    {
                        var difference = a.digits[i] + b.digits[i] + carryover;
                        if (difference < 0)
                        {
                            difference += 10;
                            carryover = 1;
                        }
                        else
                        {
                            carryover = 0;
                        }

                        result.Add(difference);
                    }
                    for (var i = minLengthOfDigit; i < maxLengthOfDigit; i++)
                    {

                        var difference = compareResult == 1 ? a.digits[i] + carryover : b.digits[i] + carryover;
                        if (difference < 0)
                        {
                            difference += 10;
                            carryover = 1;
                        }
                        else
                        {
                            carryover = 0;
                        }

                        result.Add(difference);
                    }
                    return new BigNumber(mark, result);
                }//OK
                for (int i = 0; i < minLengthOfDigit; i++)
                {
                    int sum = a.digits[i] + b.digits[i] + carryover;
                    if (sum >= 10)
                    {
                        sum -= 10;
                        carryover = 1;
                    }
                    else
                    {
                        carryover = 0;
                    }
                    result.Add(sum);
                }


                for (int i = minLengthOfDigit; i < maxLengthOfDigit; i++)
                {
                    var sum = compareResult == 1 ? a.digits[i] + carryover : b.digits[i] + carryover;
                    if (sum >= 10)
                    {
                        sum -= 10;
                        carryover = 1;
                    }
                    else
                    {
                        carryover = 0;
                    }
                    result.Add(sum);
                }
                if (carryover > 0)
                {
                    result.Add(carryover);
                }
            }//OK
            //(a + ,b -) || (a-,b+)
            if (a.mark == Mark.positive && b.mark == Mark.negative || a.mark == Mark.negative && b.mark == Mark.positive)
            {
                if (compareResult == 0)
                {
                    result.Add(0);
                    return new BigNumber(mark, result);
                }
                else if (compareResult == 1 || compareResult == -1)
                {


                    for (var i = 0; i < minLengthOfDigit; i++)
                    {
                        var difference = compareResult == 1 ? a.digits[i] - b.digits[i] - carryover : b.digits[i] - a.digits[i] - carryover;
                        if (difference < 0)
                        {
                            difference += 10;
                            carryover = 1;
                        }
                        else
                        {
                            carryover = 0;
                        }

                        result.Add(difference);
                    }
                    for (var i = minLengthOfDigit; i < maxLengthOfDigit; i++)
                    {
                        var difference = compareResult == 1 ? a.digits[i] - carryover : b.digits[i] - carryover;
                        if (difference < 0)
                        {
                            difference += 10;
                            carryover = 1;
                        }
                        else
                        {
                            carryover = 0;
                        }

                        result.Add(difference);

                    }
                    if (compareResult == -1 && b.mark == Mark.negative || compareResult == 1 && a.mark == Mark.negative)
                    {
                        mark = Mark.negative;
                    }
                }
            }
            RemoveZeros(result);
            return new BigNumber(mark, result);
        }
        public static BigNumber Subtract(BigNumber a, BigNumber b)
        {
            List<int> result = new List<int>();

            var mark = Mark.positive;
            var compareResult = CompareDigitsBySize(a, b);
            var maxLength = Math.Max(a.digits.Count, b.digits.Count);
            var minLength = Math.Min(a.digits.Count, b.digits.Count);
            var carryover = 0;


            if (a.mark == Mark.positive && b.mark == Mark.negative || a.mark == Mark.negative && b.mark == Mark.positive)
            {
                for (var i = 0; i < minLength; i++)
                {
                    var difference = a.digits[i] + b.digits[i] + carryover;
                    if (difference < 0)
                    {
                        difference += 10;
                        carryover = 1;
                    }
                    else
                    {
                        carryover = 0;
                    }

                    result.Add(difference);
                }
                for (var i = minLength; i < maxLength; i++)
                {

                    var difference = compareResult == 1 ? a.digits[i] + carryover : b.digits[i] + carryover;
                    if (difference < 0)
                    {
                        difference += 10;
                        carryover = 1;
                    }
                    else
                    {
                        carryover = 0;
                    }

                    result.Add(difference);
                }
                mark = a.mark;
                return new BigNumber(mark, result);
            }
            if (a.mark == Mark.positive && b.mark == Mark.positive || a.mark == Mark.negative && b.mark == Mark.negative)
            {

                for (var i = 0; i < minLength; i++)
                {
                    var difference = compareResult == 1 ? a.digits[i] - b.digits[i] - carryover : b.digits[i] - a.digits[i] - carryover;
                    if (difference < 0)
                    {
                        difference += 10;
                        carryover = 1;
                    }
                    else
                    {
                        carryover = 0;
                    }

                    result.Add(difference);
                }
                for (var i = minLength; i < maxLength; i++)
                {
                    var difference = compareResult == 1 ? a.digits[i] - carryover : b.digits[i] - carryover;
                    if (difference < 0)
                    {
                        difference += 10;
                        carryover = 1;
                    }
                    else
                    {
                        carryover = 0;
                    }

                    result.Add(difference);


                }
                if (compareResult == 1 && a.mark == Mark.negative && b.mark == Mark.negative)
                {
                    mark = Mark.negative;
                }
                if (compareResult == -1 && a.mark == Mark.negative && b.mark == Mark.negative)
                {
                    mark = Mark.positive;
                }
                if (compareResult == -1 && a.mark == Mark.positive && b.mark == Mark.positive)
                {
                    mark = Mark.negative;
                }

                RemoveZeros(result);
            }
            return new BigNumber(mark, result);
        }
        public static BigNumber Multiply(BigNumber a, BigNumber b)
        {
            Mark mark = a.mark == b.mark ? Mark.positive : Mark.negative;
            List<int> result = new List<int>();
            var maxLength = Math.Max(a.digits.Count, b.digits.Count);
            var minLength = Math.Min(a.digits.Count, b.digits.Count);
            if (CompareDigitsBySize(a, Zero) == 0 || CompareDigitsBySize(b, Zero) == 0)
            {
                return Zero;
            }
            int compareResult = CompareDigitsBySize(a, b);
            var carryover = 0;
            var res = 0;

            var max = compareResult == 1 ? a : b;
            var min = compareResult == 1 ? b : a;
            List<int> first = new List<int>();
            List<int> second = new List<int>();
            for (int i = 0; i < maxLength; i++)
            {
                for (int j = 0; j < minLength; j++)
                {
                    res = max.digits[i] * min.digits[j] + carryover;
                    carryover = res / 10;
                    res %= 10;
                    first.Add(res);

                }
                if (carryover != 0)
                {
                    first.Add(carryover);
                }
                carryover = 0;
                if (i == 0)
                {
                    second = first.ToList();
                    first.Clear();
                }
                BigNumber sum;
                if (i > 0)
                {
                    for (int k = 0; k < i; k++)
                    {
                        first.Insert(0, 0);
                    }
                }
                sum = Sum(new BigNumber(first), new BigNumber(second));
                result = sum.digits.ToList();
                first.Clear();
                second = sum.digits.ToList();
            }


            return new BigNumber(mark, result);
        }
        public static BigNumber Divide(BigNumber a, BigNumber b)
        {
            Mark mark = a.mark == b.mark ? Mark.positive : Mark.negative;
            if (CompareDigitsBySize(b, Zero) == 0)
            {
                throw new DivideByZeroException();
            }
            if (CompareDigitsBySize(a, b) == -1 || CompareDigitsBySize(a, Zero) == 0)
            {
                return Zero;
            }
            
            
            List<int> result = new List<int>();
            
            



            return new BigNumber(mark, result);
        }
        public override string ToString()
        {
            var result = new StringBuilder(mark == Mark.positive ? "" : "-");
            foreach (var item in digits.Reverse<int>())
            {
                result.Append(Convert.ToString(item));
            }
            return result.ToString();
        }
        public BigNumber ParseFromStringToBigNumber(string number)
        {
            mark = number.StartsWith("-") ? Mark.negative : Mark.positive;

            foreach (var item in number.StartsWith("-") ? number.Substring(1).Reverse() : number.Reverse())
            {
                digits.Add(Convert.ToInt32(item.ToString()));
            }
            return new BigNumber(mark, digits);
        }
        public static int CompareDigitsBySize(BigNumber a, BigNumber b)
        {
            if (a.digits.Count > b.digits.Count)
            {
                return 1;
            }
            else if (a.digits.Count < b.digits.Count)
            {
                return -1;
            }
            return CompareDigitIgnoreMark(a, b);
        }
        public static int CompareDigitIgnoreMark(BigNumber a, BigNumber b)
        {

            for (int i = a.digits.Count - 1; i >= 0; i--)
            {
                if (a.digits[i] > b.digits[i])
                {
                    return 1;
                }
                else if (a.digits[i] < b.digits[i])
                {
                    return -1;
                }
            }
            return 0;
        }
        public static void RemoveZeros(List<int> list)
        {
            for (var i = list.Count - 1; i > 0; i--)
            {
                if (list[i] == 0)
                {
                    list.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }
        }
    }
    //TO DO add sum different (+/-,-/+,-/-) substract( +/- , -/- , -/+)
}
