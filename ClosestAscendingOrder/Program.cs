using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ClosestAscendingOrder
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var input = "";
                do
                {
                    Console.WriteLine("Enter your number");
                    input = Console.ReadLine();
                    bool success = IsNumber(input);
                    if (!success) throw new ArgumentException("Your input must be an integer");

                    if (input != "exit")
                    {
                        var result = getClosestAccendingOrder(input.ToCharArray());
                        Console.WriteLine("The answer is: " + result);
                        Console.WriteLine("Please type exit to close the program");
                    }
                } while (input != "exit");
            }
            catch (Exception e)
            {
                Console.WriteLine("Wrong input format");
            }
        }

        static bool IsNumber(string text)
        {
            var regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(text);
        }

        public static string getClosestAccendingOrder(char[] str)
        {
            var result = new List<char>();
            // Single Digit number
            if (str.Length == 1)
            {
                return new string(str);
            }
            int i = 0;
            // Setting all digits after first mismatch to 9.
            for (; i < str.Length - 1; i++)
            {
                if (str[i] > str[i + 1])
                {
                    for (int j = i + 1; j < str.Length; j++)
                        str[j] = '9';
                    break;
                }
            }
            // Check mismatch at (i-1) and i
            if (i == str.Length - 1)
            {
                return new string(str);
            }
            else
            {
                str[i]--;
                for (int j = i; j > 0; j--)
                {
                    if (str[j - 1] > str[j])
                    {
                        str[j] = '9';
                        str[j - 1]--;
                    }
                }
                // Ignoring leading zero number
                for (i = 0; i < str.Length; i++)
                {
                    if (str[i] != '0') result.Add(str[i]);
                }

                return string.Join("", result);
            }
        }
    }
}
