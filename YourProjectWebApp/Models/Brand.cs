using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using YourProjectWebApp.DAL;

namespace YourProjectWebApp.Models
{
    public class Brand
    {
        public int BrandId { get; set; }

        // Annotations are used to apply restrictions which will help with good structure
        // and allow for input validation when creating the forms.
        [Required]
        [StringLength(50)]
        public string BrandName { get; set; }


        private const string Url = "api/brand/";


        /// <summary>
        /// HttpRequest for retrieving single item
        /// </summary>
        /// <param name="id">Id of the item</param>
        /// <returns>Object</returns>
        public static Brand GetSingle(int id)
        {
            Brand brand = null;
            // Uses the selected id as a parameter for retrieving selected brand
            using (var responseTask = ApiConnection.YourProjectApiClient.GetAsync(Url + id))
            {
                //wait for response
                responseTask.Wait();

                var result = responseTask.Result;
                // checks to see if the Get request was successful
                if (result.IsSuccessStatusCode)
                {
                    // reads the in as a brand object
                    var readTask = result.Content.ReadAsAsync<Brand>();
                    readTask.Wait();
                    // pass the result into brand
                    brand = readTask.Result;
                }
            }
            // returns null if no brand found or completed brand.
            return brand;
        }

        /// <summary>
        /// Contacts Api and Retrieves Enumeration of all brand objects stored in the database
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Brand>> GetAll()
        {
            // sends request to the API using the url specific to the api. It then closes the request connection
            // response is finished.
            using (HttpResponseMessage response = await ApiConnection.YourProjectApiClient.GetAsync(Url))
            {
                // if the response status code is successful (200)
                if (response.IsSuccessStatusCode)
                {
                    // Collects the objects in a variable 
                    var brand = await response.Content.ReadAsAsync<IEnumerable<Brand>>();
                    // passes back the object
                    return brand;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        /// <summary>
        /// sends a post request using the model to create entry
        /// </summary>
        /// <param name="brand">Object from Model Brand</param>
        /// <returns></returns>
        public static bool Create(Brand brand)
        {
            // sends a post request using the model to create entry
            using (var postTask = ApiConnection.YourProjectApiClient.PostAsJsonAsync<Brand>(Url, brand))
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
        /// <param name="brand">Object from Model Brand</param>
        /// <returns>HttpRequest results</returns>
        public static bool Update(Brand brand)
        {
            // Sends a PUT request to the api with the Model object
            using (var putTask = ApiConnection.YourProjectApiClient.PutAsJsonAsync<Brand>(Url, brand))
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
            // Attaches the Api String and brand and sends a Delete Request
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


        public static IEnumerable<SelectListItem> Convert(IEnumerable<Brand> brands)
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = "Please Select", Value = null, Selected = true}
            };

            foreach (var brand in brands)
            {
                list.Add(new SelectListItem()
                {
                    Text = brand.BrandName,
                    Value = brand.BrandId.ToString()
                });
            }

            return list;

        }
    }
}