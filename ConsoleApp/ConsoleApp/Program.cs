using System;
using System.Net;
using System.Linq;
using System.Threading;
using System.Diagnostics;

using System.IO;

namespace ConsoleApp
{
    class Program
    {
        
        static void Main()
        {
            //Sum even numbers & print to console
            //Console.WriteLine("The total sum of even numbers is " + SumNumbers());

            //Dump a given URL into console
            //Console.WriteLine(GETRequest());

            //Print numbers in list to console using 2 threads with a 500ms or 1000ms delay between print
            //ThreadHandler();

            //Ensure the Web  App is running prior running this.
            GetWebAppTime();

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

        public static void ThreadHandler()
        {
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

        public static void Thread1(Stopwatch stopWatch, int[] inputInt)
        {
            int delay = 500;

            for (int i=0; i<inputInt.Length; i++)
            {
                if (i != 0)
                {
                    Thread.Sleep(delay - (Convert.ToInt32(stopWatch.ElapsedMilliseconds) % delay));
                    //Thread.Sleep(delay);
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
                    //Thread.Sleep(delay);
                }
                //Console.WriteLine(Thread.CurrentThread.Name + ": " + inputInt[i] + ": Time elapsed: {0}", stopWatch.ElapsedMilliseconds);
                Console.WriteLine(Thread.CurrentThread.Name + ": " + inputInt[i]); 
            }
        }

        public static void GetWebAppTime()
        {
            WebClient webClient = new WebClient();
            //webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            //The Local host URL below should be static. However, if you have issues for 
            //use the instructions below:
                //Open WebApplication.sln in VS. Select the Solution Explorer tab and open Properties.
                //Select Web from the left tab and update the localhost with your Project URL below

            string localHost = "https://localhost:44342/";
            Console.WriteLine(webClient.DownloadString(localHost + "HttpHandler.aspx"));
            
            //Simulate 500 error
            Console.WriteLine(webClient.DownloadString(localHost + "HttpHandler.aspx?simulate500=Yes"));
        }
    }
}
