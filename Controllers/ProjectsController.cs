using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ICE3.Data;
using ICE3.Models;

namespace ICE3.Controllers;

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

    [HttpGet]
    public IActionResult Details(int id)
    {
        //  Database --> Retrieve project from database with specified id or returns null if not found
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound();
        }
        
        project.StartDate = DateTime.Now;   // Convert UTC to current time equivalent
        project.EndDate = DateTime.Now;     // Convert UTC to current time equivalent
        return View(project);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        //  Database --> Retrieve project from database with specified id or returns null if not found
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
        
        if (project == null) return NotFound(); 
        return View(project);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("ProjectId, Name, Description")] Project project)
    {
        // [Bind] ensures only the specified properties are updated
        if (id != project.ProjectId)
        {
            return NotFound();   // Ensures the id in the route matches the id of the passed in project
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

    // Checks for existence of project by provided id
    private bool ProjectExists(int id)
    {
        return _context.Projects.Any(e => e.ProjectId == id);
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound();
        }
        return View(project);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        
        //  Find the project by its primary key
        var project = _context.Projects.Find(id);
        if (project == null) return NotFound();
        
        _context.Projects.Remove(project);  // Remove project from database
        _context.SaveChanges();             // Commit changes to database
        return RedirectToAction("Index");
    }
    
}