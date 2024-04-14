using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication15.Areas.ProjectManagement.Models;
using WebApplication15.Data;

namespace WebApplication15.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManagement")]
    [Route("[area]/[controller]/[action]")]
    public class ProjectsController : Controller
    {
        private readonly AppDbContext _db;
        public ProjectsController(AppDbContext db)
        {

            _db = db;

        }

        /**
         * GET: Projects
         * Accessible at /Projects
        **/
        [HttpGet("")]
        // public IActionResult Index() --lab 7
        public async Task<IActionResult> Index()
        {


            //    return View(_db.Projects.ToList());

            var projects = await _db.Projects.ToListAsync(); //lab 7
            return View(projects);
        }

        /***** 
         * GET: Projects/Details/5
         * The ":int" constraint ensures id is an integer
         ***/
        [HttpGet("Details/{id:int}")]
        //public IActionResult Details(int id)
        public async Task<IActionResult> Details(int id)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }
        /****
        * GET: Projects/Create
         * */
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        /** 
         * POST: Projects/Create
         */
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Description, StartDate, EndDate, Status")] Project project) // this is added ?
        {
            if (ModelState.IsValid)
            {
                await _db.AddAsync(project);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }
        /**
         * GET: Projects/Edit/5
         */
        [HttpGet("Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var project = _db.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        /***
         * POST: Projects/Edit/5
         */
        [HttpPost("Edit/{id:int}")]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId, Name, Description, StartDate, EndDate, Status")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                     _db.Update(project);     // no await is required here
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ProjectExists(project.ProjectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        private async Task<bool> ProjectExists(int id)
        {
            return await _db.Projects.AnyAsync(e => e.ProjectId == id);
        }
        /**** 
         * GET: Projects/Delete/5
            */
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }
        /*** 
         * POST: Projects/DeleteConfirmed/5
         */
        [HttpPost("DeleteConfirmed/{id:int}")]
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ProjectId)
        {
            var project = _db.Projects.Find(ProjectId);
            if (project != null)
            {
                 _db.Projects.Remove(project);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Handle the case where the project might not be found
            return NotFound();
        }

        // Custom route for search functionality
        // Accessible at /Projects/Search/{searchString?}
        [HttpGet("Search/{searchString?}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var projectsQuery = from p in _db.Projects
                                select p;

            bool searchPerformed = !String.IsNullOrEmpty(searchString);

            if (searchPerformed)
            {
                projectsQuery = projectsQuery.Where(p => p.Name.Contains(searchString)
                                               || p.Description.Contains(searchString));
            }

            var projects = await projectsQuery.ToListAsync();
            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;
            return View("Index", projects); // Reuse the Index view to display results
        }

    }
}
