using ICE3.Areas.ProjectManagement.Models;
using ICE3.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ICE3.Areas.ProjectManagement.Controllers;

[Area("ProjectManagement")]
[Route("[area]/[controller]/[action]")]
public class ProjectCommentController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProjectController> _logger;

    public ProjectCommentController(ApplicationDbContext context, ILogger<ProjectController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetComments(int projectId)
    {
        _logger.LogInformation("Accessed ProjectCommentController GetComments at {Time}", DateTime.Now);
        var comments = await _context.Comments
            .Where(c => c.ProjectId == projectId)
            .OrderByDescending(c => c.DatePosted)
            .ToListAsync();
        
        return Json(comments);
    }

    public async Task<IActionResult> AddComment([FromBody] ProjectComment comment)
    {
        _logger.LogInformation("Accessed ProjectCommentController AddComments at {Time}", DateTime.Now);
        if (ModelState.IsValid)
        {
            comment.DatePosted = DateTime.Now;  // Add the current date/time of comment creation
            
            await _context.Comments.AddAsync(comment);  // Save the comment
            await _context.SaveChangesAsync();          // Commit to database
            
            return Json( new { success = true, message = "Comment added successfully." });
        }
        
        // Collect any errors from the Json operation
        var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage);
        _logger.LogWarning("Could not create comment on project with id {projectId} at {Time}", comment.ProjectId, DateTime.Now);
        return Json(new { success = false, message = "Invalid comment data.", errors = errors });
    }
}