using DataTransferObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        private static HttpClient client;
        static async Task Main(string[] args)
        {
            while (true)
            {
                List<Task> tasks = new List<Task>();

                client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(2);

                for (int i = 0; i < 20; i++)
                {
                    OrderDto order1 = new OrderDto() { ON = i };

                    tasks.Add(Execute(order1));
                }                

                await Task.WhenAll(tasks.ToArray());

                foreach (Task<string> task in tasks)
                {
                    Console.WriteLine(task.Result);
                }

                Console.WriteLine("Finished");

                if (Console.ReadLine() != string.Empty)
                    break;
            }
        }

        private static async Task<string> Execute(OrderDto order1)
        {
            var json = JsonConvert.SerializeObject(order1);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var resultmessage = await client.PostAsync("http://localhost:5000/orders", data);

            var result = await resultmessage.Content.ReadAsStringAsync();

            return result;
        }
    }
}
