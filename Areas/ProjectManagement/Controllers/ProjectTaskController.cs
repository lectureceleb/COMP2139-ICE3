﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ICE3.Data;
using ICE3.Areas.ProjectManagement.Models;
using ICE3.Models;

namespace ICE3.Areas.ProjectManagement.Controllers;

[Area("ProjectManagement")]
[Route("Tasks/[action]")]
public class ProjectTaskController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProjectController> _logger;

    public ProjectTaskController(ApplicationDbContext context, ILogger<ProjectController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet ("{projectId:int}")]
    public async Task<IActionResult> Index(int projectId)
    {
        _logger.LogInformation("Accessed ProjectTaskController Index at {Time}", DateTime.Now);
        // Database --> Retrieve all tasks that belong to the supplied projectId
        var tasks = await _context
            .Tasks
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();
        
        ViewBag.ProjectId = projectId;
        return View(tasks);
    }
    
    [HttpGet("{taskId:int}")]
    public async Task<IActionResult> Details(int taskId)
    {
        _logger.LogInformation("Accessed ProjectTaskController Details at {Time}", DateTime.Now);
        //  Database --> Retrieve task from database with specified id or return null if not found
        var task = await _context
            .Tasks
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.ProjectTaskId == taskId);
        
        // if (task == null) return NotFound();
        return View(task);
    }

    [HttpGet("{projectId:int}")]
    public async Task<IActionResult> Create(int projectId)
    {
        _logger.LogInformation("Accessed ProjectTaskController Create at {Time}", DateTime.Now);
        //  Database --> Retrieve task from database with specified id or return null if not found
        var project = await _context.Projects.FindAsync(projectId);
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

    [HttpPost("{projectId:int}")] //
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title, Description, ProjectId")] ProjectTask task)
    {
        _logger.LogInformation("Accessed ProjectTaskController Create at {Time}", DateTime.Now);
        // Database --> Persist new task to the database
        if (ModelState.IsValid)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            _logger.LogWarning("Could not create project task with id of {projectId} at {Time}", task.ProjectId, DateTime.Now);
            return RedirectToAction("Index", new { projectId = task.ProjectId });
        }
        return View(task);
    }

    [HttpGet, Route("{taskId:int}")]
    public async Task<IActionResult> Edit(int taskId)
    {
        _logger.LogInformation("Accessed ProjectTaskController Edit at {Time}", DateTime.Now);
        //  Database --> Retrieve task from database with specified id or return null if not found
        var task = await _context
            .Tasks
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.ProjectTaskId == taskId);
        
        if (task == null) return NotFound();
        ViewBag.TaskId = taskId;
        return View(task);
    }

    [HttpPost("{taskId:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int taskId, [Bind("ProjectTaskId, Title, Description, ProjectId")] ProjectTask task)
    {
        _logger.LogInformation("Accessed ProjectTaskController Edit at {Time}", DateTime.Now);
        if (taskId != task.ProjectTaskId) return NotFound();
        
        // Update projects if data passes server-side validation
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(task);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { projectId = task.ProjectId });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TaskExists(task.ProjectTaskId)) return NotFound();
            }
        }
        return View(task);
    }

    // Checks for existence of a task by provided the id
    private async Task<bool> TaskExists(int taskId)
    {
        return await _context.Projects.AnyAsync(e => e.ProjectId == taskId);
    }

    [HttpGet ("{taskId:int}")]
    public async Task<IActionResult> Delete(int taskId)
    {
        _logger.LogInformation("Accessed ProjectTaskController Delete at {Time}", DateTime.Now);
        var task = await _context
            .Tasks
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.ProjectTaskId == taskId);

        return (task == null) ? NotFound() : View(task);
        
        // if (task == null) return NotFound();
        // return View(task);
    }

    [HttpPost("{taskId:int}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int taskId)
    {
        _logger.LogInformation("Accessed ProjectTaskController DeleteConfirmed at {Time}", DateTime.Now);
        var task = await _context.Tasks.FindAsync(taskId);

        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            _logger.LogWarning("Could not delete project task with id of {projectId} at {Time}", task.ProjectId, DateTime.Now);
            return RedirectToAction("Index", new { projectId = task.ProjectId });
        }
        return View(task);
    }

    [HttpGet("{searchString}")]
    public async Task<IActionResult> Search(int? projectId, string searchString)
    {
        _logger.LogInformation("Accessed ProjectTaskController Search at {Time}", DateTime.Now);
        var tasksQuery = _context.Tasks.AsQueryable();
        
        bool searchPerformed = !string.IsNullOrWhiteSpace(searchString);

        if (projectId.HasValue)
        {
            tasksQuery = tasksQuery.Where(t => t.ProjectId == projectId);
        }

        if (searchPerformed)
        {
            searchString = searchString.ToLower();
            
            tasksQuery = tasksQuery.Where(p =>
                p.Title.ToLower().Contains(searchString) ||
                (p.Description != null && p.Description.ToLower().Contains(searchString)));
        }

        // Asynchronous execution means this method does not block the thread while waiting for the database
        var tasks = await tasksQuery.ToListAsync();
        
        // Pass search metadata
        ViewBag.ProjectId = projectId;
        ViewBag.SearchString = searchString;
        ViewBag.SearchPerformed = searchPerformed;
        
        return View("Index", tasks);
    }
    
}