using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace arithmeticOperationsWithVeryLargeNumbers
{
    public class BigNumber
    {
        private enum Mark
        {
            negative = 0,
            positive = 1
        }
        private Mark mark;
        List<int> digits = new List<int>();
        public BigNumber(string bigNumber)
        {
            mark = IsPositive(bigNumber) == true ? Mark.positive : Mark.negative;
            ParseFromStringToBigNumber(bigNumber);
        }
        private BigNumber(List<int> list)
        {
            mark = Mark.positive;
            digits = list;
        }

        private BigNumber(Mark mark, List<int> result)
        {
            this.mark = mark;
            digits = result;
        }

        private static readonly BigNumber Zero = new("0");
        static bool IsPositive(string number)
        {

            if (string.IsNullOrEmpty(number))
            {
                throw new ArgumentException();
            }
            return !number.StartsWith("-");
        }
        public static BigNumber Sum(BigNumber a, BigNumber b)
        {

            List<int> result = new List<int>();
            var maxLengthOfDigit = Math.Max(a.digits.Count, b.digits.Count);
            var minLengthOfDigit = Math.Min(a.digits.Count, b.digits.Count);
            int carryover = 0;
            var compareResult = CompareDigitsBySize(a, b);
            int difference = 0;
            Stack<int> stack = new Stack<int>();
            for (int i = minLengthOfDigit - 1, j = maxLengthOfDigit - 1; i >= 0; i--, j--)
            {
                if (a.digits.Count == b.digits.Count)
                {
                    difference = a.digits[i] + b.digits[i] + carryover;
                }
                else
                {
                    difference = compareResult == 1 ? a.digits[j] + b.digits[i] + carryover : a.digits[i] + b.digits[j] + carryover;
                }

                if (difference >= 10)
                {
                    difference -= 10;
                    carryover = 1;
                }
                else
                {
                    carryover = 0;
                }
                stack.Push(difference);

            }
            for (int i = maxLengthOfDigit - minLengthOfDigit - 1; i >= 0; i--)
            {

                difference = compareResult == 1 ? a.digits[i] + carryover : b.digits[i] + carryover;
                if (difference >= 10)
                {
                    difference -= 10;
                    carryover = 1;
                }
                else
                {
                    carryover = 0;
                }

                stack.Push(difference);

            }
            if (carryover > 0)
            {
                stack.Push(carryover);
            }
            while (stack.Count > 0)
            {
                result.Add(stack.Pop());
            }
            var mark = a.mark == Mark.negative ? Mark.negative : Mark.positive;



            return new BigNumber(mark, result);
        }
        public static BigNumber Substract(BigNumber a, BigNumber b)
        {
            List<int> result = new List<int>();

            var mark = Mark.positive;
            var compareResult = CompareDigitsBySize(a, b);
            var maxLength = Math.Max(a.digits.Count, b.digits.Count);
            var minLength = Math.Min(a.digits.Count, b.digits.Count);
            var carryover = 0;
            Stack<int> stack = new Stack<int>();


            if (compareResult == 0)
            {
                return Zero;
            }
            for (int i = minLength - 1, j = maxLength - 1; i >= 0; i--, j--)
            {
                var difference = compareResult == 1 ? a.digits[j] - b.digits[i] - carryover : b.digits[j] - a.digits[i] - carryover;
                if (difference < 0)
                {
                    difference += 10;
                    carryover = 1;
                }
                else
                {
                    carryover = 0;
                }
                stack.Push(difference);
            }
            for (int i = maxLength - minLength - 1; i >= 0; i--)
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
                stack.Push(difference);
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
            while (stack.Count > 0)
            {
                result.Add(stack.Pop());
            }
            RemoveZeros(result);

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
            int addZeroCount = 0;
            var max = compareResult == 1 ? a : b;
            var min = compareResult == 1 ? b : a;
            List<int> first = new List<int>();
            List<int> second = new List<int>();
            Stack<int> stack = new Stack<int>();
            for (int i = minLength - 1; i >= 0; i--)
            {
                addZeroCount++;
                if (i < minLength - 1)
                {
                    for (int k = 0; k < addZeroCount - 1; k++)
                    {
                        stack.Push(0);
                    }
                }
                for (int j = maxLength - 1; j >= 0; j--)
                {
                    res = max.digits[j] * min.digits[i] + carryover;
                    carryover = res / 10;
                    res %= 10;
                    stack.Push(res);
                }
                if (carryover != 0)
                {
                    stack.Push(carryover);
                }
                carryover = 0;
                if (i == minLength - 1)
                {
                    while (stack.Count > 0)
                    {
                        second.Add(stack.Pop());
                    }
                }
                BigNumber sum;
                sum = new BigNumber(stack.ToList()) + new BigNumber(second);
                result = sum.digits.ToList();
                second = sum.digits.ToList();
                stack.Clear();
            }
            return new BigNumber(mark, result);
        }
        public static BigNumber Divide(BigNumber a, BigNumber b)
        {
            if (CompareDigitsBySize(b, Zero) == 0)
            {
                throw new DivideByZeroException();
            }
            if (CompareDigitsBySize(a, b) == -1 || CompareDigitsBySize(a, Zero) == 0)
            {
                return Zero;
            }
            if (CompareDigitsBySize(a, b) == 0)
            {
                return new BigNumber("1");
            }
            Mark mark = a.mark == b.mark ? Mark.positive : Mark.negative;
            int countSubstraction = 0;
            var bSize = b.digits.Count;
            BigNumber newDigit = new BigNumber(Mark.positive, new List<int>());
            BigNumber remainderDivision = new BigNumber(Mark.positive, new List<int>());
            List<int> result = new List<int>();
            for (int i = 0; i < bSize; i++)
            {
                newDigit.digits.Add(a.digits[0]);
                a.digits.RemoveAt(0);
            }
            BigNumber number = new BigNumber(newDigit.digits);
            b.mark = b.mark == Mark.negative ? b.mark = Mark.positive : b.mark = Mark.positive;
            while (a.digits.Count != 0)
            {
                countSubstraction = 0;
                while (CompareDigitsBySize(number, b) != -1)
                {
                    number = number - b;
                    countSubstraction++;
                }
                remainderDivision = number;

                if (countSubstraction != 0)
                {
                    result.Add(countSubstraction);
                }
                else
                {
                    result.Add(0);
                }
                if (CompareDigitsBySize(remainderDivision, Zero) != 0 || a.digits.Count > 0)
                {
                    if (CompareDigitsBySize(remainderDivision, Zero) != 0)
                    {
                        remainderDivision.digits.Add(0);
                    }
                    number = new BigNumber(a.digits[0].ToString()) + remainderDivision;
                    a.digits.RemoveAt(0);
                }
            }
            countSubstraction = 0;
            while (CompareDigitsBySize(number, b) != -1)
            {
                number = number - b;
                countSubstraction++;
            }
            remainderDivision = number;

            if (countSubstraction != 0)
            {
                result.Add(countSubstraction);
            }
            else
            {
                result.Add(0);
            }
            RemoveZeros(result);
            return new BigNumber(mark, result);
        }
        public override string ToString()
        {
            var result = new StringBuilder(mark == Mark.positive ? "" : "-");
            result.AppendJoin("", digits);
            return result.ToString();
        }
        public BigNumber ParseFromStringToBigNumber(string number)
        {
            mark = number.StartsWith("-") ? Mark.negative : Mark.positive;

            foreach (var item in number.StartsWith("-") ? number.Substring(1) : number)
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

            for (int i = 0; i < a.digits.Count; i++)
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
            for (var i = 0; i < list.Count; i++)
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
        public static BigNumber operator -(BigNumber a)
        {
            a.mark = a.mark == Mark.positive ? Mark.negative : Mark.positive;
            return a;
        }
        public static BigNumber operator +(BigNumber a, BigNumber b) => a.mark == b.mark
        ? Sum(a, b)
        : Substract(a, b);

        public static BigNumber operator -(BigNumber a, BigNumber b) => a.mark != b.mark
        ? Sum(a, b)
        : Substract(a, b);
        public static BigNumber operator *(BigNumber a, BigNumber b) => Multiply(a, b);
        public static BigNumber operator /(BigNumber a, BigNumber b) => Divide(a, b);
    }

}
