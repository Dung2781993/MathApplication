using System;
using System.Collections.Generic;

namespace AllMediaDesk
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var input = "";
                do
                {
                    Console.WriteLine("Enter math formula");
                    input = Console.ReadLine();
                    if (input != "exit")
                    {
                        var calculator = new RegexCalculator.RegexCalculator();
                        var result = calculator.Calculate(input);
                        Console.WriteLine("The answer is: " + result);
                        Console.WriteLine("Please type exit to close the program");
                    }
                } while (input != "exit");
            } catch(Exception e)
            {
                Console.WriteLine("Wrong input format");
            }
            
        }
    }
}
