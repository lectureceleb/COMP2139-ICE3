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
    public IActionResult Index()
    {
        // Database --> Retrieve all projects from database
        var projects = _context.Projects.OrderBy(p => p.ProjectId).ToList();   // Sort by id
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
    public IActionResult Create(Project project)
    {
        // Database --> Persist new project to the database
        if (ModelState.IsValid)
        {
            project.StartDate = DateTime.UtcNow;    // Convert current time to UTC equivalent
            project.EndDate = DateTime.UtcNow;      // Convert current time to UTC equivalent
            _context.Projects.Add(project);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(project);
    }

    [HttpGet,Route("{projectId:int}")]
    public IActionResult Details(int projectId)
    {
        //  Database --> Retrieve project from database with specified id or return null if not found
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == projectId);
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
    public IActionResult Edit(int projectId, [Bind("ProjectId, Name, Description")] Project project)
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
                _context.SaveChanges();     // Commit changes
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(project.ProjectId))
                {
                    return NotFound();
                }
            }
            return RedirectToAction("Index");
        }
        return View(project);
    }

    // Checks for existence of a project by provided the id
    private bool ProjectExists(int id)
    {
        return _context.Projects.Any(e => e.ProjectId == id);
    }

    [HttpGet, Route("{projectId:int}")]
    public IActionResult Delete(int projectId)
    {
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == projectId);
        if (project == null)
        {
            return NotFound();
        }
        return View(project);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int projectId)
    {
        //  Find a project by its primary key
        var project = _context.Projects.Find(projectId);
        if (project == null) return NotFound();
        
        _context.Projects.Remove(project);  // Remove project from database
        _context.SaveChanges();             // Commit changes to database
        return RedirectToAction("Index");
    }
    
}