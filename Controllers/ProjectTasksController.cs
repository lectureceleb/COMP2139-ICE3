using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ICE3.Data;
using ICE3.Models;

namespace ICE3.Controllers;

[Route("[controller]/[action]")]
public class ProjectTasksController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProjectTasksController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet,Route("{projectId:int}")]
    public IActionResult Index(int projectId)
    {
        // Database --> Retrieve all tasks that belong to the supplied projectId
        var tasks = _context
            .Tasks
            .Where(t => t.ProjectId == projectId)
            .ToList();
        
        ViewBag.ProjectId = projectId;
        return View(tasks);
    }
    
    [HttpGet,Route("{taskId:int}")]
    public IActionResult Details(int taskId)
    {
        //  Database --> Retrieve task from database with specified id or return null if not found
        var task = _context
            .Tasks
            .Include(t => t.Project)
            .FirstOrDefault(t => t.ProjectTaskId == taskId);
        
        // if (task == null) return NotFound();
        return View(task);
    }

    [HttpGet,Route("{projectId:int}")]
    public IActionResult Create(int projectId)
    {
        //  Database --> Retrieve task from database with specified id or return null if not found
        var project = _context.Projects.Find(projectId);
        if (project == null) return NotFound();   // Or handle appropriately if project doesn't exist

        // Create a new task with the foreign key already set 
        var task = new ProjectTask
        {
            ProjectId = projectId,
            Title = "",
            Description = ""
        };
        ViewBag.ProjectId = projectId;
        return View(task);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Title, Description, ProjectId")] ProjectTask task)
    {
        // Database --> Persist new task to the database
        if (ModelState.IsValid)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return RedirectToAction("Index", new { projectId = task.ProjectId });
        }
        return View(task);
    }

    [HttpGet, Route("{taskId:int}")]
    public IActionResult Edit(int taskId)
    {
        //  Database --> Retrieve task from database with specified id or return null if not found
        var task = _context
            .Tasks
            .Include(t => t.Project)
            .FirstOrDefault(t => t.ProjectTaskId == taskId);
        
        if (task == null) return NotFound();
        @ViewBag.TaskId = taskId;
        return View(task);
    }

    [HttpPost, Route("{taskId:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int taskId, [Bind("ProjectTaskId, Title, Description, ProjectId")] ProjectTask task)
    {
        if (taskId != task.ProjectTaskId) return NotFound();
        
        // Update projects if data passes server-side validation
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(task);
                _context.SaveChanges();
                return RedirectToAction("Index", new { projectId = task.ProjectId });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(task.ProjectTaskId)) return NotFound();
            }
        }
        return View(task);
    }

    // Checks for existence of a task by provided the id
    private bool TaskExists(int taskId)
    {
        return _context.Projects.Any(e => e.ProjectId == taskId);
    }

    [HttpGet,Route("{taskId:int}")]
    public IActionResult Delete(int taskId)
    {
        var task = _context
            .Tasks
            .Include(t => t.Project)
            .FirstOrDefault(t => t.ProjectTaskId == taskId);
        
        if (task == null) return NotFound();
        return View(task);
    }

    [HttpPost, ActionName("Delete"), Route("{taskId:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int taskId)
    {
        var task = _context.Tasks.Find(taskId);

        if (task != null)
        {
            _context.Tasks.Remove(task);
            _context.SaveChanges();
            return RedirectToAction("Index", new { projectId = task.ProjectId });
        }
        return View(task);
    }
    
}