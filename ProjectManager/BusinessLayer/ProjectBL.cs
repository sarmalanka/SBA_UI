using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ProjectManager.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Text;

namespace ProjectManager.BusinessLayer
{
    public class ProjectBL
    {
        private ProjectManagerEntities1 db = new ProjectManagerEntities1();

        // GET: api/Projects
        public List<ProjectEntity> GetProjects()
        {
            List<ProjectEntity> objProjects = new List<ProjectEntity>();
            foreach(var project in db.Projects)
            {
                ProjectEntity objProject = new ProjectEntity();
                objProject = RetProjectEntity(project);
                objProjects.Add(objProject);
            }
            return objProjects;
        }

        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        public void GetProject(int id)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                //return NotFound();
            }

            //return Ok(project);
        }

        // PUT: api/Projects/5
        [ResponseType(typeof(void))]
        public bool PutProject(int id, ProjectEntity project)
        {
            Project objProject = new Project();
            objProject = RetProject(project);
            db.Entry(objProject).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: api/Projects
        [ResponseType(typeof(Project))]
        public ProjectEntity PostProject(ProjectEntity project)
        {
            Project objProject = new Project();
            objProject = RetProject(project);
            db.Projects.Add(objProject);
            db.SaveChanges();
            return project;
        }

        // DELETE: api/Projects/5
        [ResponseType(typeof(Project))]
        public bool DeleteProject(int id)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return false;
            }

            db.Projects.Remove(project);
            db.SaveChanges();

            return true;
        }
        private ProjectEntity RetProjectEntity(Project project)
        {
            ProjectEntity objProject = new ProjectEntity();
            objProject.ProjectID = project.ProjectID;
            using (var ctx = new ProjectManagerEntities1())
            {
                var projIDParam = new SqlParameter("@ProjectID", project.ProjectID);
                var UserID = ctx.Database.SqlQuery<ProjectEntity>(
                 "exec GetUserID @ProjectID", projIDParam).ToList<ProjectEntity>();
                if (UserID != null & UserID.Count > 0)
                {
                    objProject.ManagerID = Convert.ToInt32(UserID[0].ManagerID);
                    objProject.ManagerName = db.Users.Where(x => x.UserID == objProject.ManagerID).First().FirstName;
                }
            }
            
            objProject.ProjectTitle = project.ProjectTitle;
            objProject.StartDate = project.StartDate;
            objProject.Priority = project.Priority;
            objProject.Completed = project.Completed;
            objProject.EndDate = project.EndDate;
            objProject.TotalTasks = db.Tasks.Where(x => x.ProjectID == project.ProjectID).Count();
            objProject.CompletedTasks = db.Tasks.Where(x => x.ProjectID == project.ProjectID && x.Completed == true).Count();
            return objProject;
        }
        private Project RetProject(ProjectEntity project)
        {
            Project objProject = new Project();
            objProject.ProjectID = project.ProjectID;
            objProject.ProjectTitle = project.ProjectTitle;
            objProject.StartDate = project.StartDate;
            objProject.Priority = project.Priority;
            objProject.Completed = project.Completed;
            objProject.EndDate = project.EndDate;
            return objProject;
        }
        private bool ProjectExists(int id)
        {
            return db.Projects.Count(e => e.ProjectID == id) > 0;
        }
        public bool UpdateUser(ProjectEntity project)
        {
            using (var ctx = new ProjectManagerEntities1())
            {
                var userIDParam = new SqlParameter("@UserID", project.ManagerID);
                var projIDParam = new SqlParameter("@ProjectID", project.ProjectID);
                var Project = ctx.Database.SqlQuery<ProjectEntity>(
                 "exec UpdateUser @UserID,@ProjectID", userIDParam, projIDParam).ToList<ProjectEntity>();
            }
            return true;
        }
    }
}