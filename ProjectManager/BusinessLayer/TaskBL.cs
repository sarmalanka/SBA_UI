using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ProjectManager;
using ProjectManager.Models;
using System.Data.SqlClient;
using System.Text;

namespace ProjectManager.BusinessLayer
{
    public class TaskBL
    {
        private ProjectManagerEntities1 db = new ProjectManagerEntities1();

        // GET: api/Tasks
        public List<TaskEntity> GetTasks()
        {
            List<TaskEntity> objTasks = new List<TaskEntity>();
            foreach(var task in db.Tasks)
            {
                TaskEntity objTask = new TaskEntity();
                objTask = RetTaskEntity(task);
                objTasks.Add(objTask);
            }
            return objTasks;
        }

        // GET: api/Tasks/5
        [ResponseType(typeof(Task))]
        public IHttpActionResult GetTask(int id)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                //return NotFound();
            }

            return null;
        }

        // PUT: api/Tasks/5
        [ResponseType(typeof(void))]
        public bool PutTask(int id, TaskEntity task)
        {
            Task objTask = new Task();
            objTask = RetTask(task);
            db.Entry(objTask).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        // POST: api/Tasks
        [ResponseType(typeof(Task))]
        public TaskEntity PostTask(TaskEntity task)
        {
            Task objTask = new Task();
            objTask = RetTask(task);
            db.Tasks.Add(objTask);
            db.SaveChanges();
            return task;
        }

        // DELETE: api/Tasks/5
        [ResponseType(typeof(Task))]
        public bool DeleteTask(int id)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return false;
            }

            db.Tasks.Remove(task);
            db.SaveChanges();

            return true;
        }
        private TaskEntity RetTaskEntity(Task task)
        {
            TaskEntity objTask = new TaskEntity();
            objTask.EndDate = task.EndDate;
            objTask.ParentTaskID = task.ParentTaskID;
            var parentTask = db.ParentTasks.Where(x => x.ParentID == task.ParentTaskID);
            if (parentTask != null && parentTask.Count() > 0)
            {
                objTask.ParentTaskID = parentTask.First().ParentID;
                objTask.ParentTaskTitle = parentTask.First().ParentTaskTitle;
            }
            objTask.Priority = task.Priority;
            objTask.ProjectID = task.ProjectID;
            var project = db.Projects.Where(x => x.ProjectID == task.ProjectID);
            if (project != null && project.Count() > 0)
                objTask.ProjectTitle = project.First().ProjectTitle;
            objTask.StartDate = task.StartDate;
            objTask.TaskID = task.TaskID;
            objTask.TaskTitle = task.TaskTitle;
            var user = db.Users.Where(x => x.TaskID == task.TaskID);
            if (user != null && user.Count() > 0)
            {
                objTask.UserFirstName = user.First().FirstName;
                objTask.UserID = user.First().UserID;
            }
            objTask.Completed = task.Completed == null ? false : task.Completed;
            return objTask;
        }
        private Task RetTask(TaskEntity task)
        {
            Task objTask = new Task();
            objTask.EndDate = task.EndDate;
            objTask.ParentTaskID = task.ParentTaskID;
            objTask.Priority = task.Priority;
            objTask.ProjectID = task.ProjectID;
            //var project = db.Projects.Where(x => x.ProjectID == task.ProjectID).First();
            //if (project != null)
              //  objTask.ProjectID = project.ProjectID;
            objTask.StartDate = task.StartDate;
            objTask.TaskID = task.TaskID;
            objTask.TaskTitle = task.TaskTitle;
            objTask.Completed = task.Completed == null ? false : task.Completed;
            //var user = db.Users.Where(x => x.TaskID == task.TaskID).First();
            //if (user != null)
              //  objTask.UserID = user.UserID;
            return objTask;
        }
        
        private bool TaskExists(int id)
        {
            return db.Tasks.Count(e => e.TaskID == id) > 0;
        }
        public bool UpdateUser(TaskEntity task)
        {
            using (var ctx = new ProjectManagerEntities1())
            {
                var userIDParam = new SqlParameter("@UserID", task.UserID);
                var taskIDParam = new SqlParameter("@TaskID", task.TaskID);
                var Project = ctx.Database.SqlQuery<ProjectEntity>(
                 "exec UpdateUserIDforTask @UserID,@TaskID", userIDParam, taskIDParam).ToList<ProjectEntity>();
            }
            return true;
        }
        public bool UpdateParentTask(TaskEntity task)
        {
            using (var ctx = new ProjectManagerEntities1())
            {
                var taskIDParam = new SqlParameter("@TaskID", task.TaskID);
                var parenttaskIDParam = new SqlParameter("@ParentTaskID", task.ParentTaskID);
                var Project = ctx.Database.SqlQuery<ProjectEntity>(
                 "exec UpdateParentTask @TaskID,@ParentTaskID", taskIDParam, parenttaskIDParam).ToList<ProjectEntity>();
            }
            return true;
        }
        public bool UpdateProject(TaskEntity task)
        {
            using (var ctx = new ProjectManagerEntities1())
            {
                var taskIDParam = new SqlParameter("@TaskID", task.TaskID);
                var projectIDParam = new SqlParameter("@ProjectID", task.ProjectID);
                var Project = ctx.Database.SqlQuery<ProjectEntity>(
                 "exec UpdateProject @TaskID,@ProjectID", taskIDParam, projectIDParam).ToList<ProjectEntity>();
            }
            return true;
        }
    }
}