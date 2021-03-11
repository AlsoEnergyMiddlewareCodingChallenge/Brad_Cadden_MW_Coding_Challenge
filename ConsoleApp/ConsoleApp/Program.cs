using System;
using System.Collections.Generic;
using System.Net;
//using System.Text.RegularExpressions;
using System.Linq;
using System.Threading;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Sum even numbers & print to console
            Console.WriteLine("The total sum of even numbers is " + SumNumbers());

            //Dump a given URL into console
            Console.WriteLine(GETRequest());

            //Print numbers in list to console
            Console.WriteLine(PrintList());

        }

        public static string GETRequest()
        {
            Console.WriteLine("Please provide a URL");
            string url = Console.ReadLine();

            //May use below for URL validation later...
            //string userInput = Console.ReadLine();
            //string expr = @"/((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)/";
            //MatchCollection mc = Regex.Matches(userInput, expr);
            //Console.WriteLine(mc);

            WebClient webClient = new WebClient();
            //webClient.QueryString.Add("param1", "value1");
            string result = webClient.DownloadString(url);
            return result;
        }

        public static int SumNumbers()
        {
            int sumNumbers = 0;
            bool flag = true;
            while(flag)
            {
                try
                {
                    Console.WriteLine("Please provide a set of integers separated by commas.");
                    string userInput = Console.ReadLine();
                    var numbers = userInput.Split(',')?.Select(Int32.Parse).ToList();

                    foreach (var number in numbers)
                    {
                        if (number % 2 == 0)
                        {
                            sumNumbers = sumNumbers + number;
                        }
                    }
                    flag = false;
                }
                catch
                {
                    Console.WriteLine("Please verify all items are numbers and separated by a comma");
                    flag = true;
                }
            }
            return sumNumbers;
        }
        public static string PrintList()
        {
            Thread.Sleep(5);
            Thread.Sleep(10);
        }
    }
}
