
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using TaskManager.Model;


namespace TaskManager.Pages
{

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;

        [BindProperty]
        public string selectedFilter { get; set; }

        [BindProperty]
        public int Priority { get; set; }

        [BindProperty]
        public string Status { get; set; }

        [BindProperty]
        public int Priorityval { get; set; }

        [BindProperty]
        public string Statusval { get; set; }


        [BindProperty]
        public string TaskName { get; set; }

        [BindProperty]
        public int Selectedpriority { get; set; }

        [BindProperty]
        public string Selectedstatus { get; set; }



        public List<Model.Tasks> tasklist = new List<Model.Tasks>();

        public string ActionResultMessageText { get; set; }

        public int ActionResultPriorityText { get; set; }
        public string ActionResultErrorMessageText { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }


        public void OnGet()
        {

            Fetchdata();

        }
        [HttpPost]
        public void OnPost()
        {

            Fetchdata();

        }
        public async void OnGetSearch()
        {

            Fetchdata();

        }
        /// <summary>
        /// Update Priority ans status based on Taskname
        /// </summary>
        /// <returns></returns>

        public async Task<IActionResult> OnPostEditdata()
        {



            Tasks task = new Tasks();
            task.Name = TaskName;
            task.Priority = Priorityval;
            task.Status = Statusval;


            TaskItem objtask = new(_config);

            var response = objtask.UpdateTask(task);
            if (response == "update failed")
            {




            }
            else
            {

                Fetchdata();

            }


            return RedirectToPage("./Index");


            //Tasks objtask = new Tasks();
            //objtask.Name = TaskName;
            //objtask.Priority = Priorityval;
            //objtask.Status = Statusval;

            //var jsonContent = JsonConvert.SerializeObject(objtask);
            //var contentData = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            //var url = _config.GetSection("Apiurl").Value;

            //using (var client = new HttpClient())
            //{

            //    var HttpResponseMessage = await client.PutAsync(url + "api/Task", contentData);

            //    Fetchdata();


            //}
            //return RedirectToPage("./Index");



        }
        /// <summary>
        /// Delete data based on name
        /// </summary>
        /// <returns></returns>

        public async Task<IActionResult> OnPostDeletedata()
        {
            Tasks objtask = new Tasks();
            objtask.Name = TaskName;
            objtask.Priority = Priority;
            objtask.Status = Status;

            var jsonContent = JsonConvert.SerializeObject(objtask);
            var contentData = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var url = _config.GetSection("Apiurl").Value;

            using (var client = new HttpClient())
            {

                var HttpResponseMessage = await client.DeleteAsync(url + "api/Task/" + objtask.Name);               
                Fetchdata();


            }
            return RedirectToPage("./Index");


        }

        /// <summary>
        /// Opens the popup page for edit/delete
        /// </summary>
        /// <param name="LogId"></param>
        public void OnPostUpdatedata(string LogId)
        {
            Fetchdata();

            var url = _config.GetSection("Apiurl").Value;
            this.ActionResultMessageText = string.Empty;
            this.ActionResultErrorMessageText = LogId;

            if (RouteData.Values["LogId"] != null)
            {
                string LogId1 = (RouteData.Values["LogId"].ToString());

            }

            var tsk = tasklist.Where(u => u.Name == LogId).FirstOrDefault();
            TaskName = tsk.Name;
            Priorityval = tsk.Priority;
            Statusval = tsk.Status;





        }
        /// <summary>
        /// Search and load grid based on priority and status
        /// </summary>

        //  public async Task<IActionResult> OnPostSearchdata()
        public async void OnPostSearchdata()
        {
            var url = _config.GetSection("Apiurl").Value;

            using (var client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri(url + "api/Task");
                request.Method = HttpMethod.Get;
                HttpResponseMessage res = client.Send(request);
                var responseStr = res.Content.ReadAsStringAsync();
               

                this.tasklist = JsonConvert.DeserializeObject<List<Tasks>>(responseStr.Result);

                if (Selectedpriority > 0) { this.tasklist = this.tasklist.Where(u => u.Priority == Selectedpriority).ToList(); }
                if (!string.IsNullOrEmpty(Selectedstatus)) { this.tasklist = this.tasklist.Where(u => u.Status == Selectedstatus).ToList(); }

               

            }
            
        }


        /// <summary>
        /// Call to the get APi method
        /// </summary>


        public void Fetchdata()
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
                if (Priority > 0) { tasklist = tasklist.Where(u => u.Priority == Priority).ToList(); }
                if (!string.IsNullOrEmpty(Status)) { tasklist = tasklist.Where(u => u.Status == Status).ToList(); }


              ;
                var statusCode = res.StatusCode;
                
            }
        }

        public void OnPostopenpopup()
        {
            this.ActionResultMessageText = string.Empty;
            this.ActionResultErrorMessageText = "Success";

        }


    }

}
