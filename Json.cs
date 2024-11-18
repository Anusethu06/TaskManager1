
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using TaskManager.Model;


namespace TaskManager
{
    public class Json
    {
        private readonly IConfiguration _config;
        public List<TasksId> getJsonFileDate(string url)
        {

            // var url = _config.GetSection("Apiurl").Value;
            List<Model.TasksId> taskList = new List<Model.TasksId>();

            using (var client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri(url + "api/Task");
                request.Method = HttpMethod.Get;
                HttpResponseMessage res = client.Send(request);
                var responseStr = res.Content.ReadAsStringAsync();
                taskList = JsonConvert.DeserializeObject<List<TasksId>>(responseStr.Result);
            }
            return taskList;
        }

        public async Task<string> AddJsondatatofile(TasksId tasksid, string url)
        {
            string ActionResultMessageText = string.Empty;

            var jsonContent = JsonConvert.SerializeObject(tasksid);
            var contentData = new StringContent(jsonContent, Encoding.UTF8, "application/json");


            var _httpClient = new HttpClient();

            // Send the POST request
            HttpResponseMessage response = await _httpClient.PostAsync(url + "api/Task", contentData);

            // Read and display the response
            string responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

                ActionResultMessageText = "Record created";

            }
            else
            {
                ActionResultMessageText = "Record creation failed";
            }
            return ActionResultMessageText;
        }

        public async Task<String> UpdateJsondatatofile(TasksId taskid, string Url)
        {


            var jsonContent = JsonConvert.SerializeObject(taskid);
            var contentData = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            //  var url = _config.GetSection("Apiurl").Value;

            string response;

            using (var client = new HttpClient())
            {

                var HttpResponseMessage = await client.PutAsync(Url + "api/Task", contentData);


                if (HttpResponseMessage.IsSuccessStatusCode)
                {

                    response = "successfully updated";


                }
                else
                {

                    response = "update failed";

                }
            }

            return (response);



        }





    }
}
