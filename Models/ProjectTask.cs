using System.ComponentModel.DataAnnotations;

namespace ICE3.Models;

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
    [Required]
    public required string Title { get; set; }

    /// <summary>
    /// Mandatory task description
    /// </summary>
    [Required]
    public required string Description { get; set; }
    
    
    // Navigation properties
    
    /// <summary>
    /// Foreign key of the project this task belongs to
    /// </summary>
    public required int ProjectId { get; set; }
    
    // This property allows for easy access to related Project entity from the Task entity
    /// <summary>
    /// The project this task belongs to
    /// </summary>
    public Project? Project { get; set; }
}