using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace YourProjectWebApp.DAL
{
    public static class ApiConnection
    {
        public static HttpClient YourProjectApiClient { get; set; }

        public static void InitializeClient()
        {
            // Initializes Client and sets the address of the API
            YourProjectApiClient = new HttpClient {BaseAddress = new Uri("http://localhost:59636/") };
            // clears the request headers
            YourProjectApiClient.DefaultRequestHeaders.Accept.Clear();
            // adds heading that will request Json format which will be used to parse into our existing models
            YourProjectApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

    }
}