using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManager.Models
{
    public class ProjectEntity
    {
        public int? ProjectID { get; set; }
        public string ProjectTitle { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Priority { get; set; }

        public bool? Completed { get; set; }
        public int? TotalTasks { get; set; }
        public int? CompletedTasks { get; set; }
        public int? ManagerID { get; set; }
        public string ManagerName { get; set; }
    }
}