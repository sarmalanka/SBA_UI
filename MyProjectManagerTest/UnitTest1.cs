using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectManager.Controllers;
using ProjectManager.Models;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Linq;


namespace MyProjectManagerTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ProjectsController pCon = new ProjectsController();
            JArray projArray = pCon.GetProjects();
            List<ProjectEntity> objProjects = new List<ProjectEntity>();
            foreach (var project in db.Projects)
            {
                ProjectEntity objProject = new ProjectEntity();
                objProject = RetProjectEntity(project);
                objProjects.Add(objProject);
            }
            var json = JsonConvert.SerializeObject(objProjects);
            Assert.AreEqual(json, projArray);
        }
        private ProjectEntity RetProjectEntity(Project project)
        {
            ProjectEntity objProject = new ProjectEntity();
            objProject.ProjectID = project.ProjectID;
            using (var ctx = new ProjectManagerEntities())
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
    }
}
