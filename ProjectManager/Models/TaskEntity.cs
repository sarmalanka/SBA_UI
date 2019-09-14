using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManager.Models
{
    public class TaskEntity
    {
        public int? ProjectID { get; set; }
        public string ProjectTitle { get; set; }
        public string TaskTitle { get; set; }
        public int? TaskID { get; set; }
        public int? ParentTaskID { get; set; }
        public string ParentTaskTitle { get; set; }
        public string UserFirstName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Priority { get; set; }
        public int? UserID { get; set; }
        public bool? Completed { get; set; }
    }
}