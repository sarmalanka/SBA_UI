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
    public class ParentTasksController : ApiController
    {
        private ProjectManagerEntities1 db = new ProjectManagerEntities1();
        ParentTaskBL objParentBL = new ParentTaskBL();
        // GET: api/ParentTasks
        public JArray GetParentTasks()
        {
            try
            {
                var json = JsonConvert.SerializeObject(objParentBL.GetParentTasks());
                JArray jsonArray = JArray.Parse(json);
                return jsonArray;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: api/ParentTasks/5
        [ResponseType(typeof(ParentTask))]
        public IHttpActionResult GetParentTask(int id)
        {
            ParentTask parentTask = db.ParentTasks.Find(id);
            if (parentTask == null)
            {
                return NotFound();
            }

            return Ok(parentTask);
        }

        // POST: api/ParentTasks
        [ResponseType(typeof(ParentTask))]
        public IHttpActionResult PostParentTask([FromBody]JObject Jparenttask)
        {
            ParentTaskEntity parentTask = JsonConvert.DeserializeObject<ParentTaskEntity>(Jparenttask.ToString());
            objParentBL.PostParentTask(parentTask);
            return Ok(parentTask);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ParentTaskExists(int id)
        {
            return db.ParentTasks.Count(e => e.ParentID == id) > 0;
        }
    }
}