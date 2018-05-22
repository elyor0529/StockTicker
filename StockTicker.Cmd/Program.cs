using System;
using System.Threading;
using StockTicker.Cmd.StockServiceReference;

namespace StockTicker.Cmd
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var stockProxy = new StockWebServiceSoapClient();
            var credentials = new UserCredentials
            {
                UserName = "elyor@outlook.com",
                Password = "123456"
            };
            var counter = 0;

            Console.WriteLine("-------------------------------------");
            Console.WriteLine("The prices should auto refresh every 10 seconds.");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine(); 

            while (true)
            {
                counter++;

                var result = counter == 1
                    ? stockProxy.GetList(credentials)
                    : stockProxy.UpdatePrices(credentials);
                if (result.Success)
                {
                    Console.WriteLine("----------------{0}-------------------",counter);

                    foreach (var item in result.Result)
                    {
                        Console.WriteLine("{0}-{1}({2:C})", item.Symbol, item.Name, item.Price);
                    }

                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine(result.Error);
                }


                Thread.Sleep(10 * 1000);
            }
        }
    }
}