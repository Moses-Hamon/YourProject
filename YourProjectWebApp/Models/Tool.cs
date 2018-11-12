﻿using System;
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


        public static bool Create(Tool tool)
        {

            using (var postTask = ApiConnection.YourProjectApiClient.PostAsJsonAsync<Tool>(Url, tool))
            {
                postTask.Wait();

                var result = postTask.Result;
                // Returns the success of the Post
                return result.IsSuccessStatusCode;
            }
        }

        public static bool Update(Tool tool)
        {
            
            using (var putTask = ApiConnection.YourProjectApiClient.PutAsJsonAsync<Tool>(Url, tool))
            {
                putTask.Wait();

                var result = putTask.Result;

                return result.IsSuccessStatusCode;

            }
        }

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