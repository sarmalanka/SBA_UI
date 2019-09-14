using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ProjectManager;
using ProjectManager.Models;
using ProjectManager.BusinessLayer;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ProjectManager.Controllers
{
    public class TasksController : ApiController
    {
        private ProjectManagerEntities1 db = new ProjectManagerEntities1();
        TaskBL objTaskBL = new TaskBL();
        // GET: api/Tasks
        public JArray GetTasks()
        {
            try
            {
                var json = JsonConvert.SerializeObject(objTaskBL.GetTasks());
                JArray jsonArray = JArray.Parse(json);
                return jsonArray;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: api/Tasks/5
        [ResponseType(typeof(Task))]
        public IHttpActionResult GetTask(int id)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // PUT: api/Tasks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTask(int id, [FromBody]JObject Jtask)
        {
            try
            {
                TaskEntity task = JsonConvert.DeserializeObject<TaskEntity>(Jtask.ToString());
                objTaskBL.PutTask(id, task);
                if (!string.IsNullOrEmpty(task.ParentTaskTitle))
                {
                    var parentTask = db.ParentTasks.Where(x => x.ParentTaskTitle == task.ParentTaskTitle);
                    if (parentTask != null && parentTask.Count() > 0)
                        task.ParentTaskID = parentTask.First().ParentID;
                }
                if (!string.IsNullOrEmpty(task.UserFirstName))
                {
                    var userFirstName = db.Users.Where(x => x.FirstName == task.UserFirstName);
                    if (userFirstName != null && userFirstName.Count() > 0)
                        task.UserID = userFirstName.First().UserID;
                }
                if (!string.IsNullOrEmpty(task.ProjectTitle))
                {
                    var project = db.Projects.Where(x => x.ProjectTitle == task.ProjectTitle);
                    if (project != null && project.Count() > 0)
                        task.ProjectID = project.First().ProjectID;
                }
                if (task.UserID != null)
                    objTaskBL.UpdateUser(task);
                if (task.ProjectID != null)
                    objTaskBL.UpdateProject(task);
                if (task.ParentTaskID != null)
                    objTaskBL.UpdateParentTask(task);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Tasks
        [ResponseType(typeof(Task))]
        public IHttpActionResult PostTask([FromBody]JObject Jtask)
        {
            TaskEntity task = JsonConvert.DeserializeObject<TaskEntity>(Jtask.ToString());
            objTaskBL.PostTask(task);
            task.TaskID = db.Tasks.Where(x => x.TaskTitle == task.TaskTitle).First().TaskID;
            if (!string.IsNullOrEmpty(task.ParentTaskTitle))
            {
                var parentTask = db.ParentTasks.Where(x => x.ParentTaskTitle == task.ParentTaskTitle);
                if (parentTask != null && parentTask.Count() > 0)
                    task.ParentTaskID = parentTask.First().ParentID;
            }
            if (!string.IsNullOrEmpty(task.UserFirstName))
            {
                var userFirstName = db.Users.Where(x => x.FirstName == task.UserFirstName);
                if (userFirstName != null && userFirstName.Count() > 0)
                    task.UserID = userFirstName.First().UserID;
            }
            if (!string.IsNullOrEmpty(task.ProjectTitle))
            {
                var project = db.Projects.Where(x => x.ProjectTitle == task.ProjectTitle);
                if (project != null && project.Count() > 0)
                    task.ProjectID = project.First().ProjectID;
            }
            if (task.UserID != null)
                objTaskBL.UpdateUser(task);
            if (task.ProjectID != null)
                objTaskBL.UpdateProject(task);
            if (task.ParentTaskID != null)
                objTaskBL.UpdateParentTask(task);
            return Ok(task);
        }

        // DELETE: api/Tasks/5
        [ResponseType(typeof(Task))]
        public IHttpActionResult DeleteTask(int id)
        {
            objTaskBL.DeleteTask(id);
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaskExists(int id)
        {
            return db.Tasks.Count(e => e.TaskID == id) > 0;
        }
    }
}