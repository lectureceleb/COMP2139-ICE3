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
        _logger.LogInformation("Accessed HomeController Index at {Time}", DateTime.Now);
        return View();
    }

    [HttpGet]
    public IActionResult Videos()
    {
        _logger.LogInformation("Accessed HomeController Videos at {Time}", DateTime.Now);
        return View();
    }

    [HttpGet]
    public IActionResult About()
    {
        _logger.LogInformation("Accessed HomeController About at {Time}", DateTime.Now);
        return View();
    }

    [HttpGet]
    public IActionResult GeneralSearch(string searchType, string searchString)
    {
        _logger.LogInformation("Accessed HomeController GeneralSearch at {Time}", DateTime.Now);
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
            return RedirectToAction(nameof(ProjectController.Search), "Project", new { area="ProjectManagement", searchString });
        }
        
        if (searchType == "tasks")
        {
            // Redirect to ProjectTask search
            return RedirectToAction(nameof(ProjectTaskController.Search), "ProjectTask", new { area="ProjectManagement", searchString });
        }
        
        // If search is invalid, redirect to Home page
        return RedirectToAction(nameof(Index), "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        _logger.LogError("Accessed HomeController Error at {Time}", DateTime.Now);
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}