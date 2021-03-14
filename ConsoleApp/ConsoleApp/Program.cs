﻿using System;
using System.Net;
using System.Linq;
using System.Threading;
using System.Diagnostics;

namespace ConsoleApp
{
    class Program
    {
        
        static void Main()
        {
            //Sum even numbers & print to console
            Console.WriteLine("The total sum of even numbers is " + SumNumbers());

            //Dump a given URL into console
            Console.WriteLine(GETRequest());

            //Print numbers in list to console
            int[] inputInt = { 1, 2, 3, 4, 5 };
            
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Thread t1 = new Thread(
                () => Thread1(stopWatch, inputInt))
            {
                Name = "t1"
            };
            
            Thread t2 = new Thread(
                () => Thread2(stopWatch, inputInt))
                {
                Name = "t2"
                };

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();
        }

        public static int SumNumbers()
        {
            int sumNumbers = 0;
            bool flag = true;
            while (flag)
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
                            sumNumbers += number;
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

        public static void Thread1(Stopwatch stopWatch, int[] inputInt)
        {
            int delay = 500;

            for (int i=0; i<inputInt.Length; i++)
            {
                if (i != 0)
                {
                    Thread.Sleep(delay - (Convert.ToInt32(stopWatch.ElapsedMilliseconds) % delay));
                }
                //Console.WriteLine(Thread.CurrentThread.Name+": "+inputInt[i] + ": Time elapsed: {0}", stopWatch.ElapsedMilliseconds);
                Console.WriteLine(Thread.CurrentThread.Name + ": " + inputInt[i]);
            }
        }
        public static void Thread2(Stopwatch stopWatch, int[] inputInt)
        {
            int delay = 1000;

            for (int i = 0; i < inputInt.Length; i++)
            {
                if (i != 0) {
                    Thread.Sleep(delay - (Convert.ToInt32(stopWatch.ElapsedMilliseconds) % delay));
                }
                //Console.WriteLine(Thread.CurrentThread.Name + ": " + inputInt[i] + ": Time elapsed: {0}", stopWatch.ElapsedMilliseconds);
                Console.WriteLine(Thread.CurrentThread.Name + ": " + inputInt[i]); 
            }
        }
    }
}
