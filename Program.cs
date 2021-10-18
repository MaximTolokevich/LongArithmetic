using System;
using static arithmeticOperationsWithVeryLargeNumbers.BigNumber;

namespace arithmeticOperationsWithVeryLargeNumbers
{
    class Program
    {
        static void Main(string[] args)
        {
            var firstNum = Console.ReadLine();

            var secondNum = Console.ReadLine();

            BigNumber number1 = new BigNumber();
            number1.ParseFromStringToBigNumber(firstNum);
            BigNumber number2 = new BigNumber();
            number2.ParseFromStringToBigNumber(secondNum);
            var result = BigNumber.Subtract(number1, number2);

            Console.WriteLine(result.ToString());

        }
    }
}

