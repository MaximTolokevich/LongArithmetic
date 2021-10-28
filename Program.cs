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

            BigNumber number1 = new BigNumber(firstNum);
            
            BigNumber number2 = new BigNumber(secondNum);

            var result = number1 * number2;

            Console.WriteLine(result.ToString());
            
        }
    }
}

