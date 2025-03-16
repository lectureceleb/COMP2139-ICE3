using System.ComponentModel.DataAnnotations;

namespace ICE3.Areas.ProjectManagement.Models;

public class ProjectTask
{
    /// <summary>
    /// The unique identifier for the task
    /// </summary>
    [Key]
    public int ProjectTaskId { get; set; }
    
    /// <summary>
    /// Required field describing a task's name
    /// </summary>
    [Display(Name = "Task Title")]
    [Required]
    [StringLength(100, ErrorMessage = "Project name cannot be longer than 100 characters.")]
    public required string Title { get; set; }

    /// <summary>
    /// Mandatory task description
    /// </summary>
    [Display(Name = "Task Description")]
    [Required]
    [DataType(DataType.MultilineText)]
    [StringLength(500, ErrorMessage = "Project name cannot be longer than 500 characters.")]
    public required string Description { get; set; }
    
    
    // Navigation properties
    
    /// <summary>
    /// Foreign key of the project this task belongs to
    /// </summary>
    [Display(Name = "Parent Project ID")]
    public int ProjectId { get; set; }
    
    // This property allows for easy access to related Project entity from the Task entity
    /// <summary>
    /// The project this task belongs to
    /// </summary>
    public Project? Project { get; set; }
}