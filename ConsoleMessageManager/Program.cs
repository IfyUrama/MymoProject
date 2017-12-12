using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ConsoleMessageManager
{
    class Program
    {

        static HttpClient client = new HttpClient();

        static void ShowProduct(MymoMessage mymoMessage)
        {
            Console.WriteLine($"ID: {mymoMessage.MessageID}\tTime: " +
                $"{mymoMessage.ArrivalTime}\tMessage: {mymoMessage.ReceivedMessage}");
        }

        static async Task<Uri> CreateMessageAsync(MymoMessage mymoMessage)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/MymoMessages", mymoMessage);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        static async Task<MymoMessage> GetMessageAsync(string path)
        {
            MymoMessage mymoMessage = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                mymoMessage = await response.Content.ReadAsAsync<MymoMessage>();
            }
            return mymoMessage;
        }

        static async Task<MymoMessage> UpdateMessageAsync(MymoMessage mymoMessage)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/MymoMessages/{mymoMessage.MessageID}", mymoMessage);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            mymoMessage = await response.Content.ReadAsAsync<MymoMessage>();
            return mymoMessage;
        }

        static async Task<HttpStatusCode> DeleteMessageAsync(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"api/MymoMessages/{id}");
            return response.StatusCode;
        }

        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://localhost:60528/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Create a new message
                MymoMessage mymoMessage = new MymoMessage
                {
                    ArrivalTime = DateTime.Now,
                    ReceivedMessage = "Ifeoma Urama",
                    
                };

                var url = await CreateMessageAsync(mymoMessage);
                Console.WriteLine($"Created at {url}");

                // Get the message
                mymoMessage = await GetMessageAsync(url.PathAndQuery);
                ShowProduct(mymoMessage);

                             

                // Get the updated message
                mymoMessage = await GetMessageAsync(url.PathAndQuery);
                ShowProduct(mymoMessage);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }

        
    }
}

        
        