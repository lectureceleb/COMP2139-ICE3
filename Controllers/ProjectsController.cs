using Microsoft.AspNetCore.Mvc;
using ICE3.Models;

namespace ICE3.Controllers;

public class ProjectsController : Controller
{
    public ProjectsController() {}

    [HttpGet]
    public IActionResult Index()
    {
        var projects = new List<Project>
        {
            new Project { ProjectId = 1, Name = "Project 1", Description = "Sample project" },  
            new Project { ProjectId = 2, Name = "Project 2", Description = "Another sample project" }
        };
        
        return View(projects);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Project project)
    {
        if (ModelState.IsValid)
        {
            return RedirectToAction("Index");
        }
        return View(project);
    }

    [HttpGet]
    public IActionResult Details()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Details2()
    {
        return View();
    }
    
    
}