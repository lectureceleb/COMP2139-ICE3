using System.Diagnostics;
using ICE3.Areas.ProjectManagement.Controllers;
using Microsoft.AspNetCore.Mvc;
using ICE3.Models;
using ICE3.Areas.ProjectManagement.Models;

namespace ICE3.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult GeneralSearch(string searchType, string searchString)
    {
        // Ensure searchType is not null and handles case-insensitivity
        searchType = searchType?.Trim().ToLower();
        
        // Ensure the search string is not empty
        if (string.IsNullOrWhiteSpace(searchType) || string.IsNullOrWhiteSpace(searchString))
        {
            // Redirect back to home if the search is empty
            return RedirectToAction(nameof(Index), "Home");
        }
        
        // Determine where to redirect based on search type
        if (searchType == "projects")
        {
            // Redirect to Project search
            return RedirectToAction(nameof(ProjectsController.Search), "Projects", new { area="ProjectManagement", searchString });
        }
        
        if (searchType == "tasks")
        {
            // Redirect to ProjectTask search
            return RedirectToAction(nameof(ProjectTasksController.Search), "ProjectTasks", new { area="ProjectManagement", searchString });
        }
        
        // If search is invalid, redirect to Home page
        return RedirectToAction(nameof(Index), "Home");
    }
}