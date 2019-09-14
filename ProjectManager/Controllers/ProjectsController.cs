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
    public class ProjectsController : ApiController
    {
        private ProjectManagerEntities1 db = new ProjectManagerEntities1();
        ProjectBL objProjBL = new ProjectBL();
        // GET: api/Projects
        public JArray GetProjects()
        {
            try
            {
                var json = JsonConvert.SerializeObject(objProjBL.GetProjects());
                JArray jsonArray = JArray.Parse(json);
                return jsonArray;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        public IHttpActionResult GetProject(int id)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        // PUT: api/Projects/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProject(int id, [FromBody]JObject Jproject)
        {
            try
            {
                ProjectEntity objProject = JsonConvert.DeserializeObject<ProjectEntity>(Jproject.ToString());
                objProjBL.PutProject(id, objProject);
                if (objProject.ManagerID != null)
                    objProjBL.UpdateUser(objProject);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: api/Projects
        [ResponseType(typeof(Project))]
        public IHttpActionResult PostProject([FromBody]JObject Jproject)
        {
            ProjectEntity project = JsonConvert.DeserializeObject<ProjectEntity>(Jproject.ToString());
            objProjBL.PostProject(project);
            project.ProjectID = db.Projects.Where(x => x.ProjectTitle == project.ProjectTitle).First().ProjectID;
            if (project.ManagerID != null)
                objProjBL.UpdateUser(project);
            return Ok(project);
        }

        // DELETE: api/Projects/5
        [ResponseType(typeof(Project))]
        public IHttpActionResult DeleteProject(int id)
        {
            objProjBL.DeleteProject(id);
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

        private bool ProjectExists(int id)
        {
            return db.Projects.Count(e => e.ProjectID == id) > 0;
        }
    }
}