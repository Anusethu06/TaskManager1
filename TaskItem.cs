using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TaskManager.Model;

namespace TaskManager
{
    public class TaskItem
    {
        private readonly IConfiguration _config;
        private readonly string url;
        public TaskItem(IConfiguration configuration)
        {
            _config = configuration;
            url = _config.GetSection("Apiurl").Value;
        }

        public async Task<string> CreateTask(Tasks objtasks)
        {
            int Taskid = getTaskid();
            TasksId objTaskid = new TasksId();
            objTaskid.TaskID = Taskid;
            objTaskid.Priority = objtasks.Priority;
            objTaskid.Status = objtasks.Status;
            objTaskid.Name = objtasks.Name;
            Json objjson = new Json();
            var response = await objjson.AddJsondatatofile(objTaskid, url);
            return response;

        }


        public string UpdateTask(Tasks objtasks)
        {


            int Taskid = getTaskidbyname(objtasks.Name);
            TasksId objTaskid = new TasksId();
            objTaskid.TaskID = Taskid;
            objTaskid.Priority = objtasks.Priority;
            objTaskid.Status = objtasks.Status;
            objTaskid.Name = objtasks.Name;

            Json objjson = new Json();
            var response = objjson.UpdateJsondatatofile(objTaskid, url).ToString();
            return response;

        }

        private int getTaskidbyname(string name)
        {

            Json objjson = new Json();
            List<Model.TasksId> taskList = new List<Model.TasksId>();
            taskList = objjson.getJsonFileDate(url);

            int count = taskList.Count(u => u.Name == name);
            Model.TasksId task = new Model.TasksId();
            if (count == 1)
            {
                task = taskList.FirstOrDefault(u => u.Name == name);

            }

            return task.TaskID;

        }

        private int getTaskid()
        {

            int taskid = getLatestaskid() + 1;


            return taskid;
        }
        private int getLatestaskid()
        {
            Json objjson = new Json();
            List<Model.TasksId> taskList = new List<Model.TasksId>();
            taskList = objjson.getJsonFileDate(url);


            // Get the maximum value from the Value column
            int maxValue = taskList.Max(item => item.TaskID);

            return maxValue;




        }

        private void deletetasks(Tasks objtasks)
        {
            throw new NotImplementedException();
            // Need to implement.
        }
    }
            










    }
