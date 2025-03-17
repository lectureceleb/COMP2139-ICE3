using ICE3.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ICE3.Areas.ProjectManagement.Components.ProjectSummary;

public class ProjectSummaryViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;

    public ProjectSummaryViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync(int projectId)
    {
        var project = await _context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.ProjectId == projectId);
        
        return (project == null) ? Content("Project not found.") : View(project);
        
        if (project == null) return Content("Project not found.");
        return View(project);
    }
}