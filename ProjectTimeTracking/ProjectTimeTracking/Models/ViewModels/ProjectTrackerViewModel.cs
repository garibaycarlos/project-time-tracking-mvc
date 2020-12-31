using System.Collections.Generic;

namespace ProjectTimeTracking.Models.ViewModels
{
    public class ProjectTrackerViewModel
    {
        public IEnumerable<Category> CategoryList { get; set; }
        public IEnumerable<Resource> ResourceList { get; set; }
        public IEnumerable<ProjectStatus> ProjectStatusList { get; set; }
        public ProjectTracker ProjectTracker { get; set; }
    }
}