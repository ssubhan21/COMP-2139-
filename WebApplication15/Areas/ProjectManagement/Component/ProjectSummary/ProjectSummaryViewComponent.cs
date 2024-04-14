using Microsoft.AspNetCore.Mvc;
using WebApplication15.Areas.ProjectManagement.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication15.Data;

namespace WebApplication15.Areas.ProjectManagement.Component.ProjectSummary
{
    /**
     * Lab 6 - This class is responsible for fetching the necessary data and passing it to the view. 
     */
    public class ProjectSummaryViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public ProjectSummaryViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int projectId)
        {
            var project = await _context.Projects
                                        .Include(p => p.Tasks)
                                        .FirstOrDefaultAsync(p => p.ProjectId == projectId);

            // Handle the case when the project is not found
            if (project == null)
            {
                return Content("Project not found.");
            }

            return View(project);
        }
    }
}