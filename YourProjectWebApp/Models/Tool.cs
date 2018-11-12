using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Mvc;
using YourProjectWebApp.DAL;

namespace YourProjectWebApp.Models
{
    public class Tool
    {
        /// <summary>
        /// Properties
        /// </summary>
        public int ToolId { get; set; }

        [Required]
        public int BrandId { get; set; }

        [Required]
        public string Description { get; set; }
        public bool Active { get; set; }
        public string Comments { get; set; }
        public bool InUse { get; set; }

        private const string Url = "api/tool/";

        /// <summary>
        /// HttpRequest for retrieving single item
        /// </summary>
        /// <param name="id">Id of the item</param>
        /// <returns>Object</returns>
        public static Tool GetSingle(int id)
        {
            Tool tool = null;
            // Uses the selected id as a parameter for retrieving selected tool
            using (var responseTask = ApiConnection.YourProjectApiClient.GetAsync(Url+id))
            {
                //wait for response
                responseTask.Wait();

                var result = responseTask.Result;
                // checks to see if the Get request was successful
                if (result.IsSuccessStatusCode)
                {
                    // reads the in as a tool object
                    var readTask = result.Content.ReadAsAsync<Tool>();
                    readTask.Wait();
                    // pass the result into tool
                    tool = readTask.Result;
                }
            }
            // returns null if no tool found or completed tool.
            return tool;
        }

        /// <summary>
        /// Contacts Api and Retrieves Enumeration of all Tool objects stored in the database
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Tool>> GetAll()
        {
            
            // sends request to the API using the url specific to the api. It then closes the request connection
            // response is finished.
            using (HttpResponseMessage response = await ApiConnection.YourProjectApiClient.GetAsync(Url))
            {
                // if the response status code is successful (200)
                if (response.IsSuccessStatusCode)
                {
                    var tool = await response.Content.ReadAsAsync<IEnumerable<Tool>>();
                    return tool;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        /// <summary>
        /// Sends a Post request
        /// </summary>
        /// <param name="tool">Object from Model Tool</param>
        /// <returns></returns>
        public static bool Create(Tool tool)
        {
            // sends a post request using the model to create entry
            using (var postTask = ApiConnection.YourProjectApiClient.PostAsJsonAsync<Tool>(Url, tool))
            {
                // waits for response
                postTask.Wait();

                var result = postTask.Result;
                // Returns the success of the Post
                return result.IsSuccessStatusCode;
            }
        }

        /// <summary>
        /// Sends a PUT request to the api with the Model object
        /// </summary>
        /// <param name="tool">Object from Model tool</param>
        /// <returns>HttpRequest results</returns>
        public static bool Update(Tool tool)
        {
            // Sends a PUT request to the api with the Model object
            using (var putTask = ApiConnection.YourProjectApiClient.PutAsJsonAsync<Tool>(Url, tool))
            {
                //waits for response
                putTask.Wait();
                // records the result
                var result = putTask.Result;
                // returns the result
                return result.IsSuccessStatusCode;

            }
        }

        /// <summary>
        ///  Sends HttpRequest for Deleting Item
        /// </summary>
        /// <param name="id">Id of the item</param>
        /// <returns>HttpRequest results</returns>
        public static bool Delete(int id)
        {
            // Attaches the Api String and Tool and sends a Delete Request
            using (var deleteTask = ApiConnection.YourProjectApiClient.DeleteAsync(Url + id))
            {
                //waits for response
                deleteTask.Wait();
                // stores result
                var result = deleteTask.Result;
                // returns if the deletion was successful 
                return result.IsSuccessStatusCode;
            }
        }

     
    }



}