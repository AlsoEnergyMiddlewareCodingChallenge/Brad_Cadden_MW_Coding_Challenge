//Note, this code was tested on VS 2019 and has not been compatibility checked for previous VS versions.

using System;
using System.Net;
using System.Linq;
using System.Threading;
using System.Diagnostics;

//used for accessing Azure SQL Server. 
//In NuGet Package Manager, select the Browse tab, then search, select, and install Microsoft.Data.SqlClient.
using Microsoft.Data.SqlClient;

namespace ConsoleApp
{
    class Program
    {
        
        static void Main()
        {
            //Scenario 1 - 200 - returns everything
            //Scenario 2 - 500 - return sum even, dumps URL, and thread handling but errors 
            //Scenario 3 - timeout
            int scenario = 1;

            //Sum even numbers & print to console
            Console.WriteLine("The total sum of even numbers is " + SumNumbers());

            //Dump a given URL into console
            Console.WriteLine(GETRequest());

            //Print numbers in list to console using 2 threads with a 500ms or 1000ms delay between print
            ThreadHandler();

            //Ensure the Web  App is running prior running this.
            //(DateTime startTimeUTC, DateTime endTimeUTC, int httpStatusCode, string dataString, int status, string statusString) = GetWebAppTime(scenario);
            GetWebAppTime(scenario);

            //Check DB Connection
            //SQLInsert(startTimeUTC, endTimeUTC, httpStatusCode, dataString, status, statusString);

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

        public static Tuple<DateTime,DateTime,int,string,int,string> GetWebAppTime(int scenario)
        {
            int httpStatusCode = 0;
            string dataString = "";
            string url = "";
            string statusString="";
            int status = 0;

            //Forced to assign a value.
            DateTime startTimeUTC = DateTime.UtcNow;
            DateTime endTimeUTC = DateTime.UtcNow;


            WebClient webClient = new WebClient();
            //webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            //The Local host URL below should be static. However, if you have issues for 
            //use the instructions below:
            //Open WebApplication.sln in VS. Select the Solution Explorer tab and open Properties.
            //Select Web from the left tab and update the localhost with your Project URL below
            
            string localHost = "https://localhost:44342/";
            

            switch (scenario)
            {
                case (1):
                    status = 1;
                    url = localHost + "HttpHandler.aspx";

                    HttpWebRequest myHttpWebRequest1 = (HttpWebRequest)WebRequest.Create(url);
                    HttpWebResponse myHttpWebResponse1 = (HttpWebResponse)myHttpWebRequest1.GetResponse();
                    statusString = myHttpWebResponse1.StatusDescription;
                    httpStatusCode = (int)myHttpWebResponse1.StatusCode;
                    endTimeUTC = DateTime.UtcNow; 
                    dataString = webClient.DownloadString(url);
                    startTimeUTC = DateTime.Parse(dataString);
                    SQLInsert(startTimeUTC, endTimeUTC, httpStatusCode, dataString, status, statusString);
                    break;

                case (2):
                    status = 2;
                    url = localHost + "HttpHandler.aspx?simulate500=Yes";

                    try
                    {
                        HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                    }
                    catch
                    {
                        //Considered adding better error handling, but it appears that it will error out regardless, whether at console app level or database level.
                        //In a normal situation, I'd pursue a better option to capture this. However, due to the dates' not null requirments of the DB,
                        //it cannot allow capturing this activity even in a partial sense.
                        status = 2;
                        httpStatusCode = 500;
                        statusString = "Simulated 500 Error";
                        dataString = "Task failed succesfully";
                        Console.WriteLine(httpStatusCode.ToString() + " - " + statusString + " - " + dataString + " - Status: " + status);
                    }
                    break;

                case (3):
                    try
                    {
                        url = localHost + "HttpHandler.aspx";

                        HttpWebRequest myHttpWebRequest2 = (HttpWebRequest)WebRequest.Create(url);

                        //Set timeout to 1ms to ensure that a timeout is guaranteed.
                        myHttpWebRequest2.Timeout = 1;
                        HttpWebResponse myHttpWebResponse2 = (HttpWebResponse)myHttpWebRequest2.GetResponse();
                        Console.WriteLine("Timeout failed");

                    }
                    catch
                    {
                        httpStatusCode = 408;
                        status = 2;
                        statusString = "Timeout";
                        dataString = "Task failed succesfully";
                        Console.WriteLine(httpStatusCode.ToString() + " - " + statusString + " - " + dataString + " - Status: " + status);
                    }
                    break;

                default:
                    Console.WriteLine("A scenario of 1-3 was not selected");
                    break;
            }
            
            var timeStatus = Tuple.Create(startTimeUTC, endTimeUTC, httpStatusCode, dataString, status, statusString);
            return timeStatus;
        }

        public static void SQLInsert(DateTime startTimeUTC, DateTime endTimeUTC, int httpStatusCode, string dataString, int status, string statusString)
        {
            try
            {
                //Please note, I don't normally store credentials in code... 
                //For ease, transparency and thankfully a disposable account, I've decided to provide access.
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "also-energy-middleware-coding-challenge.database.windows.net";
                builder.UserID = "AlsoEnergy";
                builder.Password = "x82A5b4X5hgP27m";
                builder.InitialCatalog = "ae_code_challange";

                using SqlConnection connection = new SqlConnection(builder.ConnectionString);
                
                String sql = "INSERT INTO dbo.server_response_log VALUES(@StartTimeUTC, @EndTimeUTC, @HTTPStatusCode, @DataString, @Status, @StatusString);";

                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();

                command.Parameters.Add(new SqlParameter("StartTimeUTC", startTimeUTC));
                command.Parameters.Add(new SqlParameter("EndTimeUTC", endTimeUTC));
                command.Parameters.Add(new SqlParameter("HTTPStatusCode", httpStatusCode));
                command.Parameters.Add(new SqlParameter("DataString", dataString));
                command.Parameters.Add(new SqlParameter("Status", status));
                command.Parameters.Add(new SqlParameter("StatusString", statusString));
                command.ExecuteNonQuery();

                Console.WriteLine("New record has been added to the database");
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
