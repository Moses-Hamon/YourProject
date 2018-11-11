using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Mvc;
using YourProjectWebApp.DAL;

namespace YourProjectWebApp.Models
{
    public class Tool
    {
        public long ToolId { get; set; }
        public long BrandId { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public string Comments { get; set; }
        public bool InUse { get; set; }

        /// <summary>
        /// Contacts Api and Retrieves Enumeration of all Tool objects stored in the database
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Tool>> GetAll()
        {
            const string url = "api/tool/";

            // sends request to the API using the url specific to the api. It then closes the request connection
            // response is finished.
            using (HttpResponseMessage response = await ApiConnection.YourProjectApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<Tool> tool = await response.Content.ReadAsAsync<IEnumerable<Tool>>();
                    return tool;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }


        public static bool Create(Tool tool)
        {
            const string url = "api/tool/";

            using (var postTask = ApiConnection.YourProjectApiClient.PostAsJsonAsync<Tool>(url, tool))
            {
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static Tool Update(Tool tool)
        {
            const string url = "api/tool/";

            using (var putTask = ApiConnection.YourProjectApiClient.PutAsJsonAsync<Tool>(url, tool))
            {
                putTask.Wait();

                var result = putTask.Result;


            }
        }

    }



}