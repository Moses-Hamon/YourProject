using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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
        public static async Task<IEnumerable<Tool>> GetAllTools()
        {
            string url = "api/tool/";

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

    }



}