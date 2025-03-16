using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ICE3.Data;
using ICE3.Models;

namespace ICE3.Controllers;

[Route("[controller]/[action]")]
public class ProjectsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProjectsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Database --> Retrieve all projects from database
        var projects= await _context.Projects.OrderBy(p => p.ProjectId).ToListAsync();   // Sort by id
        
        return View(projects);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // CRUD - Create - Read - Update - Delete
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Project project)
    {
        // Database --> Persist new project to the database
        if (ModelState.IsValid)
        {
            project.StartDate = DateTime.UtcNow;    // Convert current time to UTC equivalent
            project.EndDate = DateTime.UtcNow;      // Convert current time to UTC equivalent
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(project);
    }

    [HttpGet,Route("{projectId:int}")]
    public async Task<IActionResult> Details(int projectId)
    {
        //  Database --> Retrieve project from database with specified id or return null if not found
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == projectId);
        if (project == null)
        {
            return NotFound();
        }
        
        project.StartDate = DateTime.Now;   // Convert UTC to current time equivalent
        project.EndDate = DateTime.Now;     // Convert UTC to current time equivalent
        return View(project);
    }

    [HttpGet, Route("{projectId:int}")]
    public IActionResult Edit(int projectId)
    {
        //  Database --> Retrieve project from database with specified id or return null if not found
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == projectId);
        
        if (project == null) return NotFound(); 
        return View(project);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int projectId, [Bind("ProjectId, Name, Description")] Project project)
    {
        // [Bind] ensures only the specified properties are updated
        if (projectId != project.ProjectId)
        {
            return NotFound();   // Ensures the id in the route matches the id of the injected project
        }
        
        // Update projects if data passes server-side validation
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(project);   // Update the project
                await _context.SaveChangesAsync();     // Commit changes
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProjectExists(project.ProjectId))
                {
                    return NotFound();
                }
            }
            return RedirectToAction("Index");
        }
        return View(project);
    }

    // Checks for existence of a project by provided the id
    private async Task<bool> ProjectExists(int id)
    {
        return await _context.Projects.AnyAsync(e => e.ProjectId == id);
    }

    [HttpGet, Route("{projectId:int}")]
    public async Task<IActionResult> Delete(int projectId)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == projectId);
        if (project == null)
        {
            return NotFound();
        }
        return View(project);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int projectId)
    {
        //  Find a project by its primary key
        var project = await _context.Projects.FindAsync(projectId);
        if (project == null) return NotFound();
        
        _context.Projects.Remove(project);  // Remove project from database
        await _context.SaveChangesAsync();             // Commit changes to database
        return RedirectToAction("Index");
    }

    [HttpGet, Route("{searchString?}")]
    public async Task<IActionResult> Search(string searchString)
    {
        // Fetch all projects from the database as an IQueryable collection
        // IQueryable allows us to apply filters before executing the database query
        var projectsQuery = _context.Projects.AsQueryable();
        
        // Check if a search string was provided (avoids null or empty search issues)
        bool searchPerformed = !string.IsNullOrWhiteSpace(searchString);

        if (searchPerformed)
        {
            // Convert searchString to lowercase to make the search case-insensitive
            searchString = searchString.ToLower();

            projectsQuery = projectsQuery.Where(p =>
                p.Name.ToLower().Contains(searchString) ||
                (p.Description != null && p.Description.ToLower().Contains(searchString)));
        }

        // Asynchronous execution means this method does not block the thread while waiting for the database
        var projects = await projectsQuery.ToListAsync();
        
        // Pass search metadata
        ViewBag.SearchString = searchString;
        ViewBag.SearchPerformed = searchPerformed;
        
        return View("Index", projects);
    }
    
}