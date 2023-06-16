using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdminProgram
{
    public class ApiService
    {
        private const string BaseUrl = "https://localhost:7162/api/";

        public async Task<User> GetUserData(string username, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string apiUrl = BaseUrl + "User/GetUserData";

                    // Create a JSON payload
                    var payload = new
                    {
                        username,
                        password
                    };
                    string jsonPayload = JsonSerializer.Serialize(payload);

                    // Create the HTTP request
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                    request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    // Send the request and get the response
                    HttpResponseMessage response = await client.SendAsync(request);

                    // Handle the response
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        User user = JsonSerializer.Deserialize<User>(responseContent);
                        return user;
                    }
                    else
                    {
                        // Handle the error response
                        string errorContent = await response.Content.ReadAsStringAsync();
                        // You can display the error message or handle it as needed
                        Console.WriteLine($"Error: {errorContent}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            }

            return null;
        }

        public async Task<List<User>> GetAllUsers()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string apiUrl = BaseUrl + "User/GetAllUsers";

                    // Create the HTTP request
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

                    // Send the request and get the response
                    HttpResponseMessage response = await client.SendAsync(request);

                    // Handle the response
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        List<User> users = JsonSerializer.Deserialize<List<User>>(responseContent);
                        return users;
                    }
                    else
                    {
                        // Handle the error response
                        string errorContent = await response.Content.ReadAsStringAsync();
                        // You can display the error message or handle it as needed
                        Console.WriteLine($"Error: {errorContent}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            }

            return null;
        }
    }
}
