

using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using TaskManager.Model;

namespace TaskManager.Pages
{
    public class ViewTaskDetailsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;

        [BindProperty]
        public string TaskName { get; set; }

        [BindProperty]
        public int Priority { get; set; }

        [BindProperty]
        public string Status { get; set; }

        public List<Model.Tasks> tasklist = new List<Model.Tasks>();

        public string ActionResultMessageText { get; set; }
        public string ActionResultErrorMessageText { get; set; }

        public ViewTaskDetailsModel(ILogger<IndexModel> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public async void OnGet()
        {
            var url = _config.GetSection("Apiurl").Value;
            using (var client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri(url + "api/Task");
                request.Method = HttpMethod.Get;
                HttpResponseMessage res = client.Send(request);
                var responseStr = await res.Content.ReadAsStringAsync();
                List<Tasks> task = JsonConvert.DeserializeObject<List<Tasks>>(responseStr);
                var statusCode = res.StatusCode;

            }

        }

        public async void OnPostOnSave()
        {
            Tasks objtask = new Tasks();
            objtask.Name = TaskName;
            objtask.Priority = Priority;
            objtask.Status = Status;

            bool bval = Fetchdata(objtask.Name);
            if (bval == false)
            {
                this.ActionResultMessageText = string.Empty;
                this.ActionResultErrorMessageText = "Record already exists";
            }
            else
            {
                this.ActionResultMessageText = string.Empty;
                var jsonContent = JsonConvert.SerializeObject(objtask);
                var contentData = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var url = _config.GetSection("Apiurl").Value;
                var _httpClient = new HttpClient();

                // Send the POST request
                HttpResponseMessage response = await _httpClient.PostAsync(url + "api/Task", contentData);

                // Read and display the response
                string responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {

                    this.ActionResultErrorMessageText = "Record  Created";
                }
            }


        }


        public async Task OnPostCreatedata()
        {


            TaskItem task = new TaskItem(_config);


            Tasks objtask = new Tasks();
            objtask.Name = TaskName;
            objtask.Priority = Priority;
            objtask.Status = Status;


            bool bval = Fetchdata(objtask.Name);
            if (bval == false)
            {
                this.ActionResultMessageText = string.Empty;
                this.ActionResultErrorMessageText = "Record already exists";
            }
            else
            {

                var response = await task.CreateTask(objtask);



                this.ActionResultMessageText = string.Empty;
                this.ActionResultErrorMessageText = response;




                //this.ActionResultMessageText = string.Empty;
                //var jsonContent = JsonConvert.SerializeObject(objtask);
                //var contentData = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                //var url = _config.GetSection("Apiurl").Value;
                //var _httpClient = new HttpClient();

                //// Send the POST request
                //HttpResponseMessage response = await _httpClient.PostAsync(url + "api/Task", contentData);

                //// Read and display the response
                //string responseString = await response.Content.ReadAsStringAsync();

                //if (response.IsSuccessStatusCode)
                //{
                //    this.ActionResultMessageText = string.Empty;
                //    this.ActionResultErrorMessageText = "Record created";

                //}

            }




        }
        public void OnpostModel(string name)
        {
            ViewData["Message"] = "Added";
        }
        public async void OnPostopenpopup()
        {
            this.ActionResultMessageText = string.Empty;
            this.ActionResultErrorMessageText = "Success";

        }

        public bool Fetchdata(string Name)
        {
            var url = _config.GetSection("Apiurl").Value;
            using (var client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri(url + "api/Task");
                request.Method = HttpMethod.Get;
                HttpResponseMessage res = client.Send(request);
                var responseStr = res.Content.ReadAsStringAsync();
                
                //tasklist = JsonConvert.DeserializeObject<List<Model.Tasks>>(responseStr.Result);

                tasklist = JsonConvert.DeserializeObject<List<Tasks>>(responseStr.Result);
                var TaskName = tasklist.Where(u => u.Name == Name).FirstOrDefault();
                if (TaskName == null)

                {
                    return true;
                }
                else
                {

                    return false;
                }
            }
        }


    }
}
