using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using MymoWebApi1.Models;
using System.Net;

namespace ConsoleApp
{
    class Program
    {

           static HttpClient client = new HttpClient();

            static void ShowProduct(MymoMessage mymoMessage)
            {
                Console.WriteLine($"ID: {mymoMessage.MessageID}\tTime: " +
                    $"{mymoMessage.ArrivalTime}\tMessage: {mymoMessage.ReceivedMessage}");
            }

            static async Task<Uri> CreateProductAsync(MymoMessage mymoMessage)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(
                    "api/MymoMessage", mymoMessage);
                response.EnsureSuccessStatusCode();

                // return URI of the created resource.
                return response.Headers.Location;
            }

            static async Task<MymoMessage> GetProductAsync(string path)
            {
                 MymoMessage mymoMessage = null;
                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    mymoMessage = await response.Content.ReadAsAsync<MymoMessage>();
                }
                return mymoMessage;
            }

            static async Task<MymoMessage> UpdateProductAsync(MymoMessage mymoMessage)
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(
                    $"api/MymoMessages/{mymoMessage.MessageID}", mymoMessage);
                response.EnsureSuccessStatusCode();

                // Deserialize the updated product from the response body.
                mymoMessage = await response.Content.ReadAsAsync<MymoMessage>();
                return mymoMessage;
            }

            static async Task<HttpStatusCode> DeleteProductAsync(string id)
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
                // Create a new product
                    MymoMessage mymoMessage = new MymoMessage
                    {
                        ReceivedMessage = "Hello Mymo",
                        
                    };

                    var url = await CreateProductAsync(mymoMessage);
                    Console.WriteLine($"Created at {url}");

                    // Get the product
                    mymoMessage = await GetProductAsync(url.PathAndQuery);
                    ShowProduct(mymoMessage);

                    // Update the product
                    Console.WriteLine("Updating price...");
                    mymoMessage.ReceivedMessage = Console.ReadLine();
                    await UpdateProductAsync(mymoMessage);

                    // Get the updated product
                    mymoMessage = await GetProductAsync(url.PathAndQuery);
                    ShowProduct(mymoMessage);

                    //// Delete the product
                    //var statusCode = await DeleteProductAsync(mymoMessage.MessageID);
                    //Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Console.ReadLine();
            }
        }
    }



