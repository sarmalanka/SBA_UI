using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ProjectManager;
using ProjectManager.Models;
using System.Data.SqlClient;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ProjectManager.BusinessLayer
{
    public class ParentTaskBL
    {
        private ProjectManagerEntities1 db = new ProjectManagerEntities1();
        public List<ParentTaskEntity> GetParentTasks()
        {
            List<ParentTaskEntity> objParentTasks = new List<ParentTaskEntity>();
            foreach (var task in db.ParentTasks)
            {
                ParentTaskEntity objParentTask = new ParentTaskEntity();
                objParentTask = RetParentTaskEntity(task);
                objParentTasks.Add(objParentTask);
            }
            return objParentTasks;
        }

        // GET: api/ParentTasks/5
        [ResponseType(typeof(ParentTask))]
        public IHttpActionResult GetParentTask(int id)
        {
            ParentTask parentTask = db.ParentTasks.Find(id);
            if (parentTask == null)
            {
                //return NotFound();
            }

            return null;
        }

        // POST: api/ParentTasks
        [ResponseType(typeof(ParentTask))]
        public ParentTask PostParentTask(ParentTaskEntity parentTask)
        {
            ParentTask objParentTask = new ParentTask();
            objParentTask = RetTask(parentTask);
            db.ParentTasks.Add(objParentTask);
            db.SaveChanges();
            return objParentTask;
        }

        private ParentTaskEntity RetParentTaskEntity(ParentTask parenttask)
        {
            ParentTaskEntity objParentTask = new ParentTaskEntity();
            objParentTask.ParentID = parenttask.ParentID;
            objParentTask.ParentTaskTitle = parenttask.ParentTaskTitle;
            return objParentTask;
        }
        private ParentTask RetTask(ParentTaskEntity task)
        {
            ParentTask objParentTask = new ParentTask();
            objParentTask.ParentID = task.ParentID;
            objParentTask.ParentTaskTitle = task.ParentTaskTitle;
            return objParentTask;
        }

        private bool ParentTaskExists(int id)
        {
            return db.ParentTasks.Count(e => e.ParentID == id) > 0;
        }
    }
}