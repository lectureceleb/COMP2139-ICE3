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
        var projects = _context.Projects.ToList();
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
    
}