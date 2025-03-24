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

    public ProjectCommentController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetComments(int projectId)
    {
        var comments = await _context.Comments
            .Where(c => c.ProjectId == projectId)
            .OrderByDescending(c => c.DatePosted)
            .ToListAsync();
        
        return Json(comments);
    }

    public async Task<IActionResult> AddComment ([FromBody] ProjectComment comment)
    {
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
        return Json(new { success = false, message = "Invalid comment data.", errors = errors });
    }
}